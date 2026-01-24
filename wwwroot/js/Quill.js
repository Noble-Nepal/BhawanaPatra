let quill;

window.initializeQuill = function () {
    const container = document.getElementById('quill-editor');
    if (!container) return;

    quill = new Quill(container, {
        theme: 'snow',
        modules: {
            toolbar: [
                [{ header: [1, 2, 3, false] }],
                ['bold', 'italic', 'underline'],
                [{ list: 'ordered' }, { list: 'bullet' }],
                ['link'],
                ['clean']
            ]
        }
    });
};

window.setQuillContent = function (html) {
    if (quill) {
        quill.setContents([]);
        quill.clipboard.dangerouslyPasteHTML(html);
    }
};

window.getQuillContent = function () {
    if (quill) {
        return quill.root.innerHTML;
    }
    return "";
};

window.destroyQuill = function () {
    if (quill) {
        quill = null;
    }
};
