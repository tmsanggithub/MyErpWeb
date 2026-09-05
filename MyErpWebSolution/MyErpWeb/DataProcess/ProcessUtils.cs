using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Web;
using iTextSharp.text.pdf;

namespace WebRunDragon.DataProcess
{
    public enum MyCachePriority
    {
        Default,
        NotRemovable
    }

    public class BusinessMemCache
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        // Gets a reference to the default MemoryCache instance. 
        private static ObjectCache cache = MemoryCache.Default;
        private CacheItemPolicy policy = null;
        private CacheEntryRemovedCallback callback = null;

        /// <summary>
        /// 1. Add session to memcatch
        /// 2. Save to DB
        /// </summary>
        /// <param name="cacheKeyName"></param>
        /// <param name="cacheItem"></param>
        /// <param name="myCacheItemPriority"></param>
        /// <param name="FilePath"></param>
        public void AddToMemCache(string cacheKeyName, Object cacheItem,
            MyCachePriority myCacheItemPriority)
        {
            try
            {
                callback = new CacheEntryRemovedCallback(this.MyCachedItemRemovedCallback);
                policy = new CacheItemPolicy();
                policy.Priority = (myCacheItemPriority == MyCachePriority.Default) ?
                        CacheItemPriority.Default : CacheItemPriority.NotRemovable;
                //Get session timeout from config
                policy.SlidingExpiration = TimeSpan.FromSeconds(AppSettingUtil.GetSessionTimeOut());
                policy.RemovedCallback = callback;
                // Add inside cache 
                cache.Set(cacheKeyName, cacheItem, policy);
            }
            catch (Exception e)
            {
                log.Error("AddToMemCache(" + cacheKeyName + ")" + e.StackTrace.ToString(), e);
            }
        }


        public bool Contains(string key)
        {
            return cache.Contains(key);
        }

        /// <summary>
        /// 1. Get object from memcache
        /// 2. Get from DB if redirect to other server
        /// </summary>
        /// <param name="cacheKeyName"></param>
        /// <returns></returns>
        public Object GetMemCachedItem(string cacheKeyName)
        {
            try
            {
                if (cache.Contains(cacheKeyName))
                {
                    return cache[cacheKeyName];
                }
            }
            catch (Exception e)
            {
                log.Error(e.StackTrace.ToString(), e);
            }
            return null;

        }

        public string GetMemCachedItemString(string cacheKeyName)
        {
            try
            {
                if (cache.Contains(cacheKeyName))
                {
                    var obj = cache[cacheKeyName];
                    if (obj != null)
                    {
                        return (string)obj;
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e.StackTrace.ToString(), e);
            }
            return null;

        }

        /// <summary>
        /// Remove session from memcache
        /// </summary>
        /// <param name="cacheKeyName"></param>
        public void RemoveMyCachedItem(string cacheKeyName)
        {
            if (cache.Contains(cacheKeyName))
            {
                cache.Remove(cacheKeyName);
            }
        }

        /// <summary>
        /// Remove action callback            
        /// </summary>
        /// <param name="arguments"></param>
        private void MyCachedItemRemovedCallback(CacheEntryRemovedArguments arguments)
        {
            try
            {
                // Log these values from arguments list 
                String strLog = String.Concat("Reason: ", arguments.RemovedReason.ToString(), " | Key-Name: ", arguments.CacheItem.Key, " | Value-Object: ", arguments.CacheItem.Value.ToString());
                log.Info(strLog);
            }
            catch (Exception e)
            {
                log.Error(e.StackTrace.ToString(), e);
            }
        }

        /// <summary>
        /// Add to memory not removable
        /// </summary>
        /// <param name="language"></param>
        /// <param name="cacheItem"></param>
        public void AddToMemCacheNotRemovable(string cacheKeyName, Object cacheItem)
        {
            try
            {
                policy = new CacheItemPolicy();
                policy.Priority = CacheItemPriority.NotRemovable;
                cache.Set(cacheKeyName, cacheItem, policy);
            }
            catch (Exception e)
            {
                log.Error(e.StackTrace.ToString(), e);
            }
        }

        /// <summary>
        /// 1. Add info to memcache auto remove
        /// 2. Save to DB
        /// </summary>
        /// <param name="cacheKeyName"></param>
        /// <param name="cacheItem"></param>
        /// <param name="myCacheItemPriority"></param>
        /// <param name="FilePath"></param>
        public void AddToMemCacheAutoRemove(string cacheKeyName, Object cacheItem,

            MyCachePriority myCacheItemPriority, long autoRemoveSeconds)
        {
            try
            {
                policy = new CacheItemPolicy();
                policy.Priority = (myCacheItemPriority == MyCachePriority.Default) ?
                        CacheItemPriority.Default : CacheItemPriority.NotRemovable;
                //Get session timeout from config
                policy.SlidingExpiration = TimeSpan.FromSeconds(autoRemoveSeconds);
                // Add inside cache 
                cache.Set(cacheKeyName, cacheItem, policy);
            }
            catch (Exception e)
            {
                log.Error(e.StackTrace.ToString(), e);
            }
        }
    }
    public class AppSettingUtil
    {
        public static long GetSessionTimeOut()
        {
            return long.Parse(ConfigurationManager.AppSettings["SessionTimeOut"].ToString());
        }

        public static string FCMKey
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["FCM_Key"]?.ToString();
                }
                catch (Exception er)
                {
                    return "AAAACoLsjC4:APA91bHBLpig9tZEcN6MJf6M1FNesztuIgWKCE60zmREA307xnbzWMx1zi6uv_PZyqyKGrsKm0qvM2Xv1jAePFYvjMrwJ-5Qb7b79vn9dXOraOMtYfGCQqkIjGlIn__pzjXUZa1cybE6";
                }
            }
        }
        public static string FCM_SenderCode
        {
            get
            {
                try
                {
                    return
                        ConfigurationManager.AppSettings["FCM_SenderCode"].ToString();
                }
                catch (Exception er)
                {
                    return "RongVietAsset";
                }
            }
        }

        /*
        public FileStream AddDigitalSignature(Stream stream)
        {
            //Load the PDF document to be signed
            GcPdfDocument doc_tenant = new GcPdfDocument();
            doc_tenant.Load(stream);

            //Init a test certificate:
            var pfxPath = Path.Combine("Resources", "Misc", "GcPdfTest.pfx");
            X509Certificate2 cert = new X509Certificate2(File.ReadAllBytes(pfxPath), "qq",
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);

            //Add Digital Signature

            //Configure Signature properties
            SignatureProperties spt = new SignatureProperties();
            spt.Certificate = cert;
            spt.Location = "Los Angeles, California";
            spt.SignerName = "Kevin Jade";

            //Init a signature field to hold the signature:            
            SignatureField sf_tenant = new SignatureField();
            sf_tenant.Widget.Rect = new RectangleF(51, 610, 149, 42);
            sf_tenant.Widget.Page = doc_tenant.Pages[1];
            sf_tenant.Widget.BackColor = Color.LightSeaGreen;
            sf_tenant.Widget.DefaultAppearance.Font = StandardFonts.Helvetica;

            // Add the signature field to the document:
            doc_tenant.AcroForm.Fields.Add(sf_tenant);

            //Connect the signature field and signature props:
            spt.SignatureField = sf_tenant;

            // Sign and save the document:
            // NOTES:
            // - Signing and saving is an atomic operation, the two cannot be separated.
            // - The stream passed to the Sign() method must be readable.
            FileStream tenant_signed = new FileStream("TenantSigned.pdf", FileMode.Create);
            doc_tenant.Sign(spt, tenant_signed);

            return tenant_signed;
        }

        public FileStream AddVisualSignature(Stream stream)
        {
            //Load the PDF document to be signed
            GcPdfDocument doc_landlord = new GcPdfDocument();
            doc_landlord.Load(stream);

            //Init a test certificate:
            var pfxPath = Path.Combine("Resources", "Misc", "GcPdfTest.pfx");
            X509Certificate2 cert = new X509Certificate2(File.ReadAllBytes(pfxPath), "qq",
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);

            //Add Visual Signature

            //Configure Signature properties
            SignatureProperties spl = new SignatureProperties();
            spl.Certificate = cert;
            spl.Location = "Los Angeles, California";
            spl.SignerName = "Kevin Jade";
            //Add an image representing the signature:
            spl.SignatureAppearance.Image = Image.FromFile(Path.Combine("Resources", "Misc", "LandlordSign.png"));
            spl.SignatureAppearance.CaptionImageRelation = GrapeCity.Documents.Pdf.Annotations.CaptionImageRelation.ImageOnly;

            //Init a signature field to hold the signature:            
            SignatureField sf_landlord = new SignatureField();
            sf_landlord.Widget.Rect = new RectangleF(348, 610, 150, 42);
            sf_landlord.Widget.Page = doc_landlord.Pages[1];
            sf_landlord.Widget.BackColor = Color.LightSeaGreen;
            sf_landlord.Widget.DefaultAppearance.Font = StandardFonts.Helvetica;

            // Add the signature field to the document:
            doc_landlord.AcroForm.Fields.Add(sf_landlord);

            //Connect the signature field and signature props:
            spl.SignatureField = sf_landlord;

            // Sign and save the document:
            // NOTES:
            // - Signing and saving is an atomic operation, the two cannot be separated.
            // - The stream passed to the Sign() method must be readable.
            FileStream landlord_signed = new FileStream("LandlordSigned.pdf", FileMode.Create);
            doc_landlord.Sign(spl, landlord_signed);

            return landlord_signed;
        }
        */
    }

    public class ProcessPdf
    {
        public static void Signature()
        {
            var outputPdfStream = new MemoryStream();
            string pathPdf = System.Web.Hosting.HostingEnvironment.MapPath("/FilesImport/UI_App_TaiSan.pdf");
            var _reader = new iTextSharp.text.pdf.PdfReader(pathPdf);
            var stamper = new iTextSharp.text.pdf.PdfStamper(_reader, outputPdfStream);
            //As iTextSharp.text.Image
            var picpath = System.Web.Hosting.HostingEnvironment.MapPath("~/FilesImport/ChuKy.png");//  Server.MapPath("signature/signature01.png")
            var image1 = iTextSharp.text.Image.GetInstance(picpath);
            image1.ScalePercent(52.0F);
            image1.SetAbsolutePosition((iTextSharp.text.PageSize.A4.Width - image1.Width), 80);
            try
            {
                iTextSharp.text.pdf.PdfContentByte waterMark;
                waterMark = stamper.GetOverContent(1);
                waterMark.AddImage(image1);
            }
            catch (Exception er)
            {

            }
            stamper.FormFlattening = true;
            stamper.Close();
            _reader.Close();
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("content-length", outputPdfStream.ToArray.Length.ToString());
            //Response.AddHeader("Content-Disposition", "attachment; filename=file.pdf");
            //Response.BinaryWrite(outputPdfStream.ToArray());
            //Response.Flush();
            //Response.End();
        }
    }
}