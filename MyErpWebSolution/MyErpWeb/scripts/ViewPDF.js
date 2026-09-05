window.onload = function () {
    //GcPdfViewer.LicenseKey = "***key***";
    let viewer = new GcPdfViewer("#viewer", {});
    viewer.addDefaultPanels();
    viewer.open("/FilesImport/UI_App_TaiSan.pdf");
}