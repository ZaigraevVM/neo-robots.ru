
CKEDITOR.plugins.add('iscomparetexts', {
	requires: "dialog,fakeobjects",
	init: function (b) {

		var command = b.addCommand('iscomparetexts',
			new CKEDITOR.dialogCommand('iscomparetexts')
		);

		command.modes = { wysiwyg: 1, source: 0 };
		command.canUndo = true;

		b.ui.addButton('iscomparetexts', {
			label: 'Сравнение текстов',
			command: 'iscomparetexts',
			icon: '/js/ckeditor/plugins/iscomparetexts/images/man_ico.png'
		});

		CKEDITOR.dialog.add('iscomparetexts', this.path + 'dialogs/iscomparetexts.js');
	}
});
