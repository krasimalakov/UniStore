function setUrl(html, status, xhr) {
    var requestedUrl = this.href;
    //var pageHtml= function () {
    //    return `<html>${$("html").html()}</html>`;
    //}
    if (window.location.href === requestedUrl) {
        history.replaceState({ html: html }, document.title, requestedUrl);
    }
    else {
        history.pushState({ html: html }, document.title, requestedUrl);
        
    }
}