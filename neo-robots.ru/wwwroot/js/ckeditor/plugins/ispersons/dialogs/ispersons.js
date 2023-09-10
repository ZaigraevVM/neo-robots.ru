CKEDITOR.dialog.add('ispersons', function (editor) {

	var searchF = function (wordsearch, dialog) {

		var dialogJ = $(dialog._.element.$);

		dialogJ.find('#cke_searchresult').html('<div class="load"></div>');
		dialogJ.find('#cke_personselected .info').html('<div class="load"></div>');

		dialogJ.find('#cke_personselected .search-info .search').html("Поиск в новостях...");
		$.ajax({
			url: "/admin/ajax/getckesearchinnews",
			data: {
				search: wordsearch,
				pageSize: dialogJ.find("#cke_pager select").val(),
				pageIndex: parseInt(dialogJ.find("#pagerIndex").val()),
				sportId: dialogJ.find(".searchsportselect select.searchsportselect").val()
			},
			context: { ws: wordsearch },
			success: function (d) {
				dialogJ.find('#cke_personselected .search-info .search').html("");

				if (d && (d.searchCount || d.searchCount == 0) && d.search == dialogJ.find('.searchtext input').val())
					dialogJ.find('#cke_personselected .search-info .search').html("В поиске инфоспорта найдено " + d.searchCount + " записей.");
			}
		});

		dialogJ.find('#cke_personselected .search-info .bd').html("Поиск в базе данных...");
		$.ajax({
			url: "/admin/ajax/getckepersonssearch",
			data: {
				search: wordsearch,
				pageSize: dialogJ.find("#cke_pager select").val(),
				pageIndex: parseInt(dialogJ.find("#pagerIndex").val()),
				sportId: dialogJ.find(".searchsportselect select.searchsportselect").val()
			},
			context: { ws: wordsearch },
			success: function (d) {
				var wsSet = this.ws;
				dialogJ.find('#cke_pager').html(d.Pager);
				dialogJ.find('#cke_searchresult').html(d.Items);

				/*
				if (d.Count == 0 && dialogJ.find('.urlinput input').val() == "")
					dialogJ.find('#cke_personselected .search-info .bd').html("В поиске инфоспорта найдено " + d.IScount + " записей.");
				else
				*/

				dialogJ.find('#cke_personselected .search-info .bd').html((d.Count > 0 ? "Персона есть в базе данных" : "Персоны нет в базе данных"));

				if (d.Count == 0 && d.IScount == 0)
					alert('Проверьте правильность введенных вами данных. Если все верно, нажмите "ОК", во избежание механических ошибок при поиске и подвязывании персон.');

				dialogJ.find('#cke_searchresult a.person').click(function () {
					dialogJ.find('#cke_personselected .name').text($(this).text());
					dialogJ.find('#cke_personselected .info').html($(this).parent().find('div.info').html());
					dialogJ.find('#cke_searchresult a.person').removeClass('active');
					$(this).addClass('active');

					dialogJ.find('.cke_dialog_ui_text.urlinput input').val('https://infosport.ru/person/' + $(this).data("personid") + '?fio=' + $(this).data("fname") + ' ' + $(this).data("lname"));

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
		try{
			var pIndex = parseInt(dialogJ.find("#pagerIndex").val());
		}
		catch(error)
		{
			console.log(error);
			return false;
		}


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

	var sportList = [];
	$.ajax({
		async: false,
		url: "/admin/sports/getlist",
		data: { },
		success: function (d) {

			var el = [];
			el.push(" - ");
			el.push(0);
			sportList.push(el);

			$.each(d.Sport, function () {
				var el = [];
				el.push(this.Name);
				el.push(this.Id);
				sportList.push(el);
			});

		}
	});

	//console.log(sportList);

	return {
		title: 'Установка ссылок на персон',
		minWidth: 500,
		minHeight: 400,
		onOk: function () {
			var cuttext = this.getValueOf('tab1', 'link');
			var href = this.getValueOf('tab1', 'url');
			//console.log('!!!href=' + href);
			if (href!="")
				this._.editor.insertHtml('<a href="' + encodeURI(href) + '">' + cuttext + '</a>');
			else
				this._.editor.insertHtml(cuttext);
		},
		onShow: function () {

			var dialogJ = $(this._.element.$);

			dialogJ.find('#cke_personselected .name').text('');
			dialogJ.find('#cke_personselected .info').html('');

			this.getButton('ok').disable();
		},
		contents: [
			{
				id: 'tab1',
				label: 'Ссылка на персону',
				title: 'Ссылка на персону',
				elements: [
					{
						type: "vbox",
						id: "urlOptions",
						children: [{
							type: "hbox",
							widths: ["50%", "50%"],
							children: [
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
									type: 'select',
									id: 'sport',
									label: 'Вид спорта',
									items: sportList,//[['Basketball','1'], ['Baseball','2'], ['Hockey','3'], ['Football','4']],
									'default': '0',
									className: 'linksportselect',
									onChange: function (api) {
										//console.log('Выбран: ' + this.getValue());
										//var dialogJ = $(this.getDialog()._.element.$);
										//dialogJ.find('.searchsportselect').val(this.getValue());
									}
								}

							],
							setup: function () {
								//this.getDialog().getContentElement("info", "linkType") || this.getElement().show()
							}
						}]
					},
					{
						id: 'url',
						type: 'text',
						label: 'URL',
						className: 'urlinput',
						onShow: function () {

							var dialogJ = $(this.getDialog()._.element.$);

							var a = this.getDialog().getParentEditor(), b = a.getSelection(), href = '';
							c = CKEDITOR.plugins.link.getSelectedLink(a);
							if (c != null && c.hasAttribute("href")) {
								b.selectElement(c);
								//href = c.getAttribute("href");
								href = decodeURI(c.getAttribute("href"));
								this.setValue(href);
								if(href!="")
									this.getDialog().getButton('ok').enable();
							}

							//console.log('sportid=' + $('#Sports').val());
							//console.log('sportidlen=' + $('#Sports').val().length);

							dialogJ.find('.searchsportselect').val(0);
							if ($('#SportsIds').val() != null && $('#SportsIds').val().length == 1) {
								//dialogJ.find('.searchsportselect').val($('#Sports').val());
								dialogJ.find('.linksportselect').val($('#SportsIds').val());
							}
							else {
								//console.log('set 0');
								dialogJ.find('.linksportselect').val(0);
							}

							dialogJ.find("#pagerIndex").val(0);

							if (href.indexOf("archive.stadium.ru/?personid=") > -1) {
								dialogJ.find('#cke_personselected .info').html('<div class="load"></div>');
								$.ajax({
									url: "/admin/ajax/getckepersonsbyid",
									data: { id: (href.split("personid=")[1]).split("&")[0] },
									success: function (d) {
										dialogJ.find('#cke_personselected .name').text(d.Name);
										dialogJ.find('#cke_personselected .info').html(d.Text);
									}
								});
							}
							else if (href.indexOf("infosport.ru/person/") > -1) {

								//infosport.ru/person/

								dialogJ.find('#cke_personselected .info').html('<div class="load"></div>');
								$.ajax({
									url: "/admin/ajax/getckepersonsbyid",
									data: { id: (href.split("infosport.ru/person/")[1]).split("?")[0] },
									success: function (d) {
										dialogJ.find('#cke_personselected .name').text(d.Name);
										dialogJ.find('#cke_personselected .info').html(d.Text);
									}
								});
							}
							else if (href.indexOf("searchall.xml") > -1) {
								dialogJ.find('#cke_personselected .info').html("");
							}
							else {
								
								if (href.indexOf('sport=') > -1) {
									//dialogJ.find('.searchsportselect').val(href.split("sport=")[1]);
									dialogJ.find('.linksportselect').val(href.split("sport=")[1]);
								}
								dialogJ.find('#cke_personselected .info').html("");
							}
						}
					},
					{
						id: 'bclear',
						type: 'button',
						label: 'Поставить на поиск',
						onClick: function () {

							var dialogJ = $(this.getDialog()._.element.$);

							dialogJ.find('#cke_searchresult a.person').removeClass('active');
							dialogJ.find('#cke_personselected .name').text('');
							dialogJ.find('#cke_personselected .info').html('');

							dialogJ.find('.cke_dialog_ui_text.urlinput input')
								.val(
									'/search?ws=' +
									this.getDialog().getValueOf('tab1', 'link') +
									(dialogJ.find("select.linksportselect").val() == 0 ? '' : '&sport=' + dialogJ.find("select.linksportselect").val())
								);
							
							
							this.getDialog().getButton('ok').enable();
						}
					},
					{
						type: 'html',
						html: '<div id="cke_personselected"><div class="name"></div><div class="search-info"><div class="bd"></div><div class="search"></div></div><div class="info"></div></div>'
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
							widths: ["40%", "40%", "20%"],
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
									type: 'select',
									id: 'sport',
									label: 'Вид спорта',
									className: 'searchsportselect',
									items: sportList,//[['Basketball','1'], ['Baseball','2'], ['Hockey','3'], ['Football','4']],
									'default': '0',
									onChange: function (api) {
										//console.log('Выбран: ' + this.getValue());
										//var dialogJ = $(this.getDialog()._.element.$);
										//dialogJ.find('.linksportselect').val(this.getValue());
									}
								},
								{
									id: 'bsearch',
									type: 'button',
									label: 'Найти',
									className: 'buttonsearch',
									onClick: function () {

										var dialogJ = $(this.getDialog()._.element.$);
										dialogJ.find("#pagerIndex").val(1);
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