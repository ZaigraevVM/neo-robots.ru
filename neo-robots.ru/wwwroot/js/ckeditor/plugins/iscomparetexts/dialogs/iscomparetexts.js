CKEDITOR.editor.prototype.getSelectedHtml = function () {
	var selection = this.getSelection();
	if (selection) {
		var bookmarks = selection.createBookmarks(),
		   range = selection.getRanges()[0],
		   fragment = range.clone().cloneContents();
		selection.selectBookmarks(bookmarks);
		var retval = "",
		   childList = fragment.getChildren(),
		   childCount = childList.count();
		for (var i = 0; i < childCount; i++) {
			var child = childList.getItem(i);
			retval += (child.getOuterHtml ?
			   child.getOuterHtml() : child.getText());
		}
		return retval;
	}
};

CKEDITOR.dialog.add('iscomparetexts', function (editor) {
	return {
		title: 'Сравнение текстов',
		width: $(window).width() - 40,
		height: $(window).height() - 120,
		minWidth: 600,
		minHeight: 400,
		onOk: function () {

		},
		onChange: function () {
			//console.log('console.log - resize');
		},
		onShow: function () {

			//$(this.getParentEditor().element.$)
			//console.log('this._.element.$');
			//console.log($(this._.element.$));
			//console.log($('.cke_editor_TextSp_dialog'));

			var dialog = $(this._.element.$);

			//Описание от пользователя
			//Описание от infosport.ru
			/*
			dialog.find('#cke_comparetexts').html('<table class="thead"><tr><td colspan="2">' + $(this.getParentEditor().element.$).data('messageforleft') + '</td><td colspan="2">' + $(this.getParentEditor().element.$).data('messageforright') + '</td></tr></table>' +
				'<div class="boxtable"><table>' +
				'<tr><td class="n1"></td><td class="l1"></td><td  class="n2"></td><td class="l2"></td></tr>' +
				'</table></div>' +
				'<div class="scroll"><div class="boxscroll"></div></div>'
				);
			*/

			dialog.find('#cke_comparetexts').html('<table class="thead"><tr><td colspan="1">' + $(this.getParentEditor().element.$).data('messageforleft') + '</td><td colspan="1">' + $(this.getParentEditor().element.$).data('messageforright') + '</td></tr></table>' +
				'<div class="boxtable"><table>' +
				'<tr><td class="l1"></td><td class="l2"></td></tr>' +
				'</table></div>' +
				'<div class="scroll" style="display:none;"><div class="boxscroll"></div></div>'
				);


			dialog.find('#cke_comparetexts').width(dialog.find('#cke_comparetexts').parents('.cke_dialog_contents_body').width() - 20);
			dialog.find('#cke_comparetexts').parents('.cke_dialog_contents_body').attr('data-width', dialog.find('#cke_comparetexts').parents('.cke_dialog_contents_body').width()).css({ padding: 0, margin: 0 });
			dialog.find('#cke_comparetexts').parents('.cke_dialog_contents_body').attr('data-height', dialog.find('#cke_comparetexts').parents('.cke_dialog_contents_body').height()).css({ padding: 0, margin: 0 });

			setInterval(function () {
				if (
					dialog.find('#cke_comparetexts').parents('.cke_dialog_contents_body').width() + '' != dialog.find('#cke_comparetexts').parents('.cke_dialog_contents_body').attr('data-width')
					|| dialog.find('#cke_comparetexts').parents('.cke_dialog_contents_body').width() + '' != dialog.find('#cke_comparetexts').parents('.cke_dialog_contents_body').attr('data-width')
				) {
					console.log(dialog.find('#cke_comparetexts').parents('.cke_dialog_contents_body').width() + '!=' + dialog.find('#cke_comparetexts').parents('.cke_dialog_contents_body').attr('data-width'));
					dialog.find('#cke_comparetexts').width(dialog.find('#cke_comparetexts').parents('.cke_dialog_contents_body').width() - 20);
					dialog.find('#cke_comparetexts').parents('.cke_dialog_contents_body').attr('data-width', dialog.find('#cke_comparetexts').parents('.cke_dialog_contents_body').width());
					dialog.find('#cke_comparetexts .boxtable').height(dialog.find('#cke_comparetexts').parents('.cke_dialog_contents_body').height() - 80);

					dialog.find('#cke_comparetexts tbody .l1 .box,#cke_comparetexts tbody .l2 .box').width(dialog.find('#cke_comparetexts').width() / 2 - 40);
					dialog.find('#cke_comparetexts .scroll').width(dialog.find('#cke_comparetexts').width() / 2 - 40);
					dialog.find('#cke_comparetexts .scroll .boxscroll').width(dialog.find('#cke_comparetexts tbody .l1 .text').width());
				}
			}, 100);

			var R = [];

			var idforcomparetexts = $(this.getParentEditor().element.$).data('idforcomparetexts');
			if (idforcomparetexts && idforcomparetexts != '' && $('#' + idforcomparetexts).length > 0)
				R = ISCompareTextsForCKE.Compare($(this.getParentEditor().element.$).val(), $('#' + idforcomparetexts).val());


			//console.log(R);

			//console.log('idforcomparetexts = ' + idforcomparetexts);
			//$('#cke_comparetexts tbody .l1').append($('<div>', { 'class': 'box' }).append($('<div>', { 'class': 'text' })));
			//$('#cke_comparetexts tbody .l2').append($('<div>', { 'class': 'box' }).append($('<div>', { 'class': 'text' })));

			dialog.find('tbody .l1').append($('<div>', { 'class': 'box' }).append($('<div>', { 'class': 'text' })));
			dialog.find('tbody .l2').append($('<div>', { 'class': 'box' }).append($('<div>', { 'class': 'text' })));


			if (R.length > 0) {
				for (var i = 0; i < R.length; i++) {
					if ('right' == $(this.getParentEditor().element.$).data('location')) {
						dialog.find('#cke_comparetexts tbody .n2').append($('<p>', { html: R[i].n1, 'class': 'status' + R[i].s1 }));
						dialog.find('#cke_comparetexts tbody .l2 .text').append($('<p>', { html: R[i].l1, 'class': 'status' + R[i].s1 }));

						dialog.find('#cke_comparetexts tbody .n1').append($('<p>', { html: R[i].n2, 'class': 'status' + R[i].s2 }));
						dialog.find('#cke_comparetexts tbody .l1 .text').append($('<p>', { html: R[i].l2, 'class': 'status' + R[i].s2 }));
					}
					else {
						dialog.find('#cke_comparetexts tbody .n1').append($('<p>', { html: R[i].n1, 'class': 'status' + R[i].s1 }));
						dialog.find('#cke_comparetexts tbody .l1 .text').append($('<p>', { html: R[i].l1, 'class': 'status' + R[i].s1 }));

						dialog.find('#cke_comparetexts tbody .n2').append($('<p>', { html: R[i].n2, 'class': 'status' + R[i].s2 }));
						dialog.find('#cke_comparetexts tbody .l2 .text').append($('<p>', { html: R[i].l2, 'class': 'status' + R[i].s2 }));
					}
				}
			}

			/*
			if (dialog.find('#cke_comparetexts tbody .l1 .text').width() < dialog.find('#cke_comparetexts tbody .l2 .text').width())
				dialog.find('#cke_comparetexts tbody .l1 .text').width(dialog.find('#cke_comparetexts tbody .l2 .text').width())
			else
				dialog.find('#cke_comparetexts tbody .l2 .text').width(dialog.find('#cke_comparetexts tbody .l1 .text').width());
			*/

			dialog.find('#cke_comparetexts .boxtable').height(dialog.find('#cke_comparetexts').parents('.cke_dialog_contents_body').height() - 80);
			dialog.find('#cke_comparetexts tbody .l1 .box, #cke_comparetexts tbody .l2 .box').width(dialog.find('#cke_comparetexts').width() / 2 - 40);

			dialog.find('#cke_comparetexts .scroll').width(dialog.find('#cke_comparetexts').width() / 2 - 40);
			dialog.find('#cke_comparetexts .scroll .boxscroll').width(dialog.find('#cke_comparetexts tbody .l1 .text').width());


			dialog.find("#cke_comparetexts .scroll").scroll(function () {
				dialog.find('#cke_comparetexts tbody .text').css({ 'left': (0 - $(this).scrollLeft()) + 'px' });
			});
		},
		contents: [
			{
				id: 'tab1',
				label: 'Тексты',
				title: 'Тексты',
				elements: [
					{
						type: 'html',
						html: '<div id="cke_comparetexts"></div>'
					}
				]
			},
		]
	};
});


