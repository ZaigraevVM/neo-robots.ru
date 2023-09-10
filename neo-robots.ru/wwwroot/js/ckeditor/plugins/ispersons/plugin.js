
CKEDITOR.plugins.add('ispersons', {
	requires: "dialog,fakeobjects",
	init: function (b) {

		var command = b.addCommand('ispersons',
			new CKEDITOR.dialogCommand('ispersons')
		);

		command.modes = { wysiwyg: 1, source: 0 };
		command.canUndo = true;

		b.ui.addButton('ispersons', {
			label: 'Ссылки на персон',
			command: 'ispersons',
			icon: '/js/ckeditor/plugins/ispersons/images/man_ico.png'
		});

		CKEDITOR.dialog.add('ispersons', this.path + 'dialogs/ispersons.js');
	}
});
