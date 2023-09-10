
CKEDITOR.plugins.add('isorganizations', {
	requires: "dialog,fakeobjects",
	init: function (b) {

		var command = b.addCommand('isorganizations',
			new CKEDITOR.dialogCommand('isorganizations')
		);

		command.modes = { wysiwyg: 1, source: 0 };
		command.canUndo = true;

		b.ui.addButton('isorganizations', {
			label: 'Ссылки на органзаций',
			command: 'isorganizations',
			icon: '/js/ckeditor/plugins/isorganizations/images/man_ico.png'
		});

		CKEDITOR.dialog.add('isorganizations', this.path + 'dialogs/isorganizations.js');
	}
});
