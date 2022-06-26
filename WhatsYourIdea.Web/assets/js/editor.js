import tinymce from 'tinymce/tinymce';

$(document).ready(function () {
    tinymce.init({
        selector: '#mytextarea',
        plugins: 'image preview autosave insertdatetime',
        images_file_types: 'jpg,png',
        file_picker_types: 'file image media',
        images_upload_url: '/editor/file',
        placeholder: 'Творите...',
        autosave_interval: '20s',
        height: '700',
        toolbar: 'restoredraft | undo redo | blocks | ' +
            'bold italic backcolor | alignleft aligncenter ' +
            'alignright alignjustify | bullist numlist outdent indent | ' +
            'removeformat | help',
        convert_urls: false
    });
    $(".js-save").on("click", function () {
        const content = tinymce.get('mytextarea').getContent();
        $('form #Text').html(content);
        $('form').submit();
    })
})