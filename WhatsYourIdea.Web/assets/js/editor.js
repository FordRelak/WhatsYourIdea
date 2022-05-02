import tinymce from 'tinymce/tinymce';

tinymce.init({
    selector: '#mytextarea',
    plugins: 'image',
    images_file_types: 'jpg,png',
    file_picker_types: 'file image media',
    images_upload_url: '/editor/file'
});