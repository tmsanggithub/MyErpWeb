using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebRunDragon.DataProcess
{
    public class ProcessAttachment
    {
        public static DataTable GetAttachments(string objectName, int objectId, string userId)
        {
            /*
             ALTER PROCEDURE [dbo].[SP_Attachments_SearchByObject]
                @ObjectName varchar(32),
                @ObjectValue int,
                @UserId nvarchar(32)
                as
             */
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("ObjectName", objectName);
            param.Add("ObjectValue", objectId);
            param.Add("UserId", userId);
            return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_Attachments_SearchByObject", CommandType.StoredProcedure, param);
        }

        public static bool AddOrUpdate(int id, string objectName, int objectId, string attName, string filePath,
            string contentType, long fileSize, string extension, string userId, string Description, string externalPath, ref int retCode)
        {
            /*
             ALTER PROCEDURE [dbo].[SP_Attachments_AddOrUpdate]
                @Id int,
                @ObjectName varchar(32),
                @ObjectValue int,
                @AttachName nvarchar(256),
                @FilePath nvarchar(512),
                @ContentType nvarchar(128),
                @FileSize int,
                @Extension varchar(8),
                @UserId nvarchar(32)
                as
             */
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Id", id);
            param.Add("ObjectName", objectName);
            param.Add("ObjectValue", objectId);
            param.Add("AttachName", attName);
            param.Add("FilePath", filePath);
            param.Add("ContentType", contentType);
            param.Add("FileSize", fileSize);
            param.Add("Extension", extension);
            param.Add("Description", Description);
            param.Add("UserId", userId);
            param.Add("ExternalPath", externalPath);
            retCode = int.Parse((DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_Attachments_AddOrUpdate_ExtPath", CommandType.StoredProcedure, param)) + "");
            return retCode > 0;
        }

        public static bool Remove(int id, string deletedPath, string userId, ref int retCode)
        {
            /*
             ALTER PROCEDURE [dbo].[SP_Attachments_Delete]
                @Id int,
                @DeletedPath nvarchar(512),
                @UserId  nvarchar(32)
                as
             */
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Id", id);
            param.Add("DeletedPath", deletedPath);
            param.Add("UserId", userId);
            retCode = int.Parse((DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_Attachments_Delete", CommandType.StoredProcedure, param)) + "");
            /*
                 0: lỗi
                -1: ko tồn tại tập tin đính kèm
                -2: ko có quyền xóa

                trường hợp xóa attachment của Issues
                -3: ko thể xóa attachment upload trước khi bị duyệt trả lại 
                -4: đang chờ duyệt thì miễn xóa, đã duyệt đang chờ phân công thì miễn xóa
                -5 -- ko thể xóa attachment upload trước thời gian duyệt cuối cùng để
                                -- tránh bị tranh cải khi đang làm mà xóa attachment
                -6: issue đã bị từ chối thì ko cho xóa
             */
            return retCode > 0;
        }

        public static DataTable GetAttachmentById(int attId, string userId)
        {

            /*
             ALTER PROCEDURE [dbo].[SP_Attachments_GetById]
                @Id int,
                @UserId nvarchar(32)
                as

            Id	AttDate	AttachName	FilePath	ContentType	FileSize	Extension	CreatedBy	CreatedTime

             */
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Id", attId);
            param.Add("UserId", userId);
            return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_Attachments_GetById", CommandType.StoredProcedure, param);
        }

        //public bool AddOrUpdateAttachment(string tableName, int attId, int issueId, string attName, string filePath,
        //    string contentType, long fileSize, string extension, string userId, string externalPath, ref int retCode)
        //{
        //    return ProcessAttachment.AddOrUpdate(attId, tableName, issueId, attName, filePath, contentType, fileSize, extension, userId, "", externalPath, ref retCode);
        //}
    }
}