CKEDITOR.dialog.add('isorganizations', function (editor) {

	var searchF = function (wordsearch, dialog) {

		var dialogJ = $(dialog._.element.$);

		dialogJ.find('#cke_searchresult').html('<div class="load"></div>');
		//dialogJ.find('#cke_organizationselected .infosearch').html('<div class="load"></div>');


		dialogJ.find('#cke_organizationselected .infosearch .search').html("Поиск в новостях...");
		$.ajax({
			url: "/admin/ajax/getckesearchinnews",
			data: {
				search: wordsearch,
				pageSize: 10,
				pageIndex: 1,
				sportId: dialogJ.find(".searchsportselect select.searchsportselect").val()
			},
			context: { ws: wordsearch },
			success: function (d) {
				dialogJ.find('#cke_organizationselected .infosearch .search').html("");

				if (d && (d.searchCount || d.searchCount == 0) && d.search == dialogJ.find('.searchtext input').val())
					dialogJ.find('#cke_organizationselected .infosearch .search').html("В поиске инфоспорта найдено " + d.searchCount + " записей.");
			}
		});


		dialogJ.find('#cke_organizationselected .infosearch .bd').html("Поиск в базе данных...");

		$.ajax({
			url: "/admin/ajax/getckeorganizationssearch",
			data: { search: wordsearch, pageSize: dialogJ.find("#cke_pager select").val(), pageIndex: parseInt(dialogJ.find("#pagerIndex").val()) },
			context: { ws: wordsearch },
			success: function (d) {
				var wsSet = this.ws;
				dialogJ.find('#cke_pager').html(d.Pager);
				dialogJ.find('#cke_searchresult').html(d.Items);

				dialogJ.find('#cke_organizationselected .infosearch .bd').html((d.Count > 0 ? "Организация есть в базе" : "Организации нет в базе"));

				/*
				dialogJ.find('#cke_organizationselected .infosearch').html((d.Count > 0 ? "Организация есть в базе" : "Организации нет в базе") +
					".<br/> В поиске инфоспорта найдено " + d.IScount + " записей.");
				*/

				dialogJ.find('#cke_searchresult a.organization').click(function () {
					var namemini = $(this).data("namemini") && $.trim($(this).data("namemini")) != '' ? $(this).data("namemini") : '';

					//dialogJ.find('#cke_organizationselected .name').text($(this).text() + (namemini != '' ? ' (' + namemini + ')' : ''));
					dialogJ.find('#cke_organizationselected .name').text($(this).text());
					dialogJ.find('#cke_organizationselected .info').html($(this).parent().find('div.info').html());
					dialogJ.find('#cke_searchresult a.organization').removeClass('active');
					dialogJ.find(this).addClass('active');

					var url = "";
					var name = namemini != '' ? $.trim(namemini) : $.trim($(this).text());
					if (5 == $(this).data("organizationsclassid"))
						url = 'https://infosport.ru/organizationsindustry/' + $(this).data("organizationid") + '?name=' + name;
					else if (4 == $(this).data("organizationsclassid"))
						url = 'https://infosport.ru/organizationsregional/' + $(this).data("organizationid") + '?name=' + name;
					else if (1 == $(this).data("organizationsclassid"))
						url = 'https://infosport.ru/organizationsinternational/' + $(this).data("organizationid") + '?name=' + name;
					else
						url = 'https://infosport.ru/organizations/' + $(this).data("organizationid") + '?name=' + name;

					dialogJ.find('.cke_dialog_ui_text.urlinput input').val(url);
					dialog.getButton('ok').enable();;
				});

				dialogJ.find("#cke_pager select").removeAttr("onchange");
				dialogJ.find("#cke_pager select").change(function () {
					dialogJ.find("#pagerIndex").val(1);
					searchF(dialogJ.find('.searchtext input').val(), dialog);
				});

				dialogJ.find('#cke_pager ul.pagination a').click(function () {

					dialogJ.find("#pagerIndex").val(getIndex(this, dialogJ));
					searchF(dialogJ.find('.searchtext input').val(), dialog);
					return false;
				});
			}
		});
	};

	var getIndex = function (e, dialogJ) {
		var pIndex = parseInt(dialogJ.find("#pagerIndex").val());
		if ($(e).is(".prev") && pIndex > 1) {
			pIndex--;
		}
		else if ($(e).is(".prevPrev")) {
			pIndex = 1;
		}
		else if ($(e).is(".next")) {
			pIndex++;
		}
		else if ($(e).is(".nextNext")) {
			pIndex = parseInt(dialogJ.find("#pagerCount").val());
		}
		else
			pIndex = parseInt($(e).text());
		return pIndex;
	};

	return {
		title: 'Установка ссылок на организацию',
		minWidth: 500,
		minHeight: 400,
		className: 'isorganizations',
		onOk: function () {
			var cuttext = this.getValueOf('tab1', 'link');
			var href = this.getValueOf('tab1', 'url');
			if (href!="")
				this._.editor.insertHtml('<a href="' + encodeURI(href) + '">' + cuttext + '</a>');
			else
				this._.editor.insertHtml(cuttext);
		},
		onShow: function () {

			this.on('resize', function (e) {
				console.log('resize');
			});

			var dialog = $(this._.element.$);

			dialog.find('#cke_organizationselected .name').text('');
			dialog.find('#cke_organizationselected .info').html('');

			this.getButton('ok').disable();
		},
		/*
		buttons: [
			CKEDITOR.dialog.okButton(editor, {
				disabled: true
			}),
			CKEDITOR.dialog.cancelButton
		],
		*/
		contents: [
			{
				id: 'tab1',
				label: 'Ссылка на организацию',
				title: 'Ссылка на организацию',
				elements: [
					{
						id: 'link',
						type: 'text',
						label: 'Текст ссылки',
						onShow: function () {
							var a = this.getDialog().getParentEditor(), b = a.getSelection(), href = '';
							c = CKEDITOR.plugins.link.getSelectedLink(a);
							if (c != null && c.hasAttribute("href")) {
								b.selectElement(c);
								href = c.getAttribute("href");
								this.setValue(c.getHtml());
							}
							else
								this.setValue(editor.getSelection().getSelectedText());

						}
					},
					{
						id: 'url',
						type: 'text',
						label: 'URL',
						className: 'urlinput',
						onShow: function () {

							var dialog = $(this.getDialog()._.element.$);

							var a = this.getDialog().getParentEditor(), b = a.getSelection(), href = '';
							c = CKEDITOR.plugins.link.getSelectedLink(a);
							if (c != null && c.hasAttribute("href")) {
								b.selectElement(c);
								href = decodeURI(c.getAttribute("href"));
								this.setValue(href);
								if(href!="")
									this.getDialog().getButton('ok').enable();
							}

							dialog.find("#pagerIndex").val(0);

							var urlorg = "";

							if (href.indexOf("infosport.ru/organizationsregional/") > -1)
								urlorg = "infosport.ru/organizationsregional/";
							else if (href.indexOf("infosport.ru/organizationsindustry/") > -1)
								urlorg = "infosport.ru/organizationsindustry/";
							else if (href.indexOf("infosport.ru/organizationsinternational/") > -1)
								urlorg = "infosport.ru/organizationsinternational/";
							else if (href.indexOf("infosport.ru/organizations/") > -1)
								urlorg = "infosport.ru/organizations/";

							if (urlorg != "")
							{
								dialog.find('#cke_organizationselected .info').html('');
								dialog.find('#cke_organizationselected .info').html('<div class="load"></div>');
								$.ajax({
									url: "/admin/ajax/getckeorganizationsbyid",
									data: { id: (href.split(urlorg)[1]).split("?")[0] },
									success: function (d) {

										if (d.Id > 0) {

											dialog.find('#cke_searchresult td a.organization[data-organizationid=' + d.Id + ']').addClass('active');

											dialog.find('#cke_organizationselected .name').text(d.Name + (d.NameMini != '' && d.NameMini != 'null' ? ' (' + d.NameMini + ')' : ''));
											dialog.find('#cke_organizationselected .info').html(
												'<div style="max-width:500px;white-space: normal;">' + d.Address + '</div>' +
												'<div style="max-width:500px;white-space: normal;">' + d.President + '</div>' +
												'<div style="max-width:500px;white-space: normal;">' + d.Text + '</div>'
											);
										}
									}
								});
							}
							else {
								dialog.find('#cke_organizationselected .info').html("");
							}
						}
					},
					{
						id: 'bclear',
						type: 'button',
						label: 'Поставить на поиск',
						onClick: function () {

							var dialog = $(this.getDialog()._.element.$);
							//%22
							dialog.find('#cke_searchresult a.organization').removeClass('active');
							dialog.find('#cke_organizationselected .name').text('');
							dialog.find('#cke_organizationselected .info').html('');
							dialog.find('.cke_dialog_ui_text.urlinput input').val('/search?ws=' + this.getDialog().getValueOf('tab1', 'link'));
							
							this.getDialog().getButton('ok').enable();
						}
					},
					{
						type: 'html',
						html: '<div id="cke_organizationselected"><div class="infosearch"><div class="bd"></div><div class="search"></div></div><div class="name"></div><div class="info"></div></div>'
					}
				]
			},
			{
				id: 'tab2',
				label: 'Поиск',
				title: 'Поиск',
				elements: [
					{
						type: "vbox",
						id: "urlOptions",
						children: [{
							type: "hbox",
							widths: ["75%", "25%"],
							children: [
								{
									id: 'searchtext',
									type: 'text',
									label: 'Поиск',
									className: 'searchtext',
									onShow: function () {
										var a = this.getDialog().getParentEditor(), b = a.getSelection(), href = '';
										c = CKEDITOR.plugins.link.getSelectedLink(a)
										if (c != null && c.hasAttribute("href")) {
											b.selectElement(c);
											href = c.getAttribute("href");
											this.setValue(c.getHtml());
										}
										else
											this.setValue(editor.getSelection().getSelectedText());

										searchF(this.getDialog().getValueOf('tab2', 'searchtext'), this.getDialog());
									}
								},
								{
									id: 'bsearch',
									type: 'button',
									label: 'Найти',
									onClick: function () {

										searchF(this.getDialog().getValueOf('tab2', 'searchtext'), this.getDialog());

									}
								}

							],
							setup: function () {
								//this.getDialog().getContentElement("info", "linkType") || this.getElement().show()
							}
						}]
					},
					{
						type: 'html',
						html: '<div id="cke_pager"></div><div id="cke_searchresult"></div>'
					}
				]
			}
		]
	};
});