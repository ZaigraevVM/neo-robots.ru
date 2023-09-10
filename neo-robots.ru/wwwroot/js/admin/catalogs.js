var catalogs = {
	init: function () {
		$("body").on("input", "input.finder", this.filterCatalog);
		$("body").on("change", "#SecFilter", this.filterCatalog);
	},
	filterCatalog: function () {
		var $el = $(this).parent();
		var filter = $el.find("input.finder").val();
		var secFilterId = $el.find("#SecFilter").val();
		$el.siblings("table").find("tbody tr").each(function (ind, el) {
			var row = $(el);
			var secFilter = true;
			if (secFilterId && secFilterId != 0) {
				if (row.data("id") != secFilterId) 
					secFilter = false;
				else 
					secFilter = true;
			}
			var ftd = row.find("td:first").text();
			if (ftd.toLowerCase().indexOf(filter.toLowerCase()) >= 0 && secFilter) {
				row.show();
			} else {
				row.hide();
			}
		});
	}
};

var catalogsFiltered = {
	callback: null,
	url: "/admin/catalogs/",
	title: "",
	filter: [],
	history: true,
	v:'',
	parent: '',
	button:'',
	init: function (params) {

		catalogsFiltered.url = params.url;
		this.callback = params.callback;
		this.title = typeof params.title == "undefined" ? "" : params.title;
		this.filter = typeof params.filter == "undefined" ? [] : params.filter;
		this.history = typeof params.history == "undefined" ? true : params.history;
		this.parent = typeof params.parent == "undefined" ? '' : params.parent;
		this.button = typeof params.button == "undefined" ? '' : params.button;

		if (this.button != '' && $(this.button).length) {
			$(this.button).click(function () {
				catalogsFiltered.getrun();
				return false;
			});
		}
		else {
			$("body").on("input", "input.finderCatalog", function () {
				var searchword = $(this).val().toString();
				setTimeout(function () {
					if (searchword == $('input.finderCatalog').val().toString())
						catalogsFiltered.get({
							url: catalogsFiltered.url,
							data: {
								pageIndex: 1,
								pageSize: 10,
								search: searchword,
								select: catalogsFiltered.getSelectVal()
							}
						});
				},
				500);
			});

			$("body").on("input", "input.finderCatalogID", function () {
				catalogsFiltered.get({
					url: catalogsFiltered.url,// + $('#getadd').val(),
					data: {
						pageIndex: 1,
						pageSize: 10,
						search: $('input.finderCatalog').val().toString(),
						select: catalogsFiltered.getSelectVal()
					}
				});
			});

			if ($("select").is("#searchSelect"))
				$("#searchSelect").change(function () {
					catalogsFiltered.get({
						url: catalogsFiltered.url,// + $('#getadd').val(),
						data: {
							pageIndex: 1,
							pageSize: 10,
							search: $("input.finderCatalog").val().toString(),
							select: catalogsFiltered.getSelectVal()
						}
					});
				});

			for (var i = 0; i < this.filter.length; i++)
				if ($('#' + this.filter[i]).attr('type') == 'text')
					$("body").on("input", "input#" + this.filter[i], function () {
						var el = $(this);
						var searchword = el.val().toString();
						setTimeout(function () {
							if (searchword == el.val().toString())
								catalogsFiltered.get({
									url: catalogsFiltered.url,
									data: {
										pageIndex: 1,
										pageSize: 10,
										search: $('input.finderCatalog').val().toString(),
										select: catalogsFiltered.getSelectVal()
									}
								});
						},
						500);
					});
				else
					$(this.parent + ' #' + this.filter[i]).change(function () {
						catalogsFiltered.get({
							url: catalogsFiltered.url,// + $('#getadd').val(),
							data: {
								pageIndex: 1,
								pageSize: 10,
								search: $('input.finderCatalog').val().toString(),
								select: catalogsFiltered.getSelectVal()
							}
						});
					});
		}

		this.initPager();
		catalogsFiltered.setHistory({ "pager": $(".adminPager").html(), Items: $(".adminItems").html() }, "", document.location);
		window.onpopstate = function (e) {
			if (e.state && typeof e.state.Pager != "undefined" && typeof e.state.Items != "undefined") {
				$(".adminPager").html(e.state.Pager);
				$(".adminItems").html(e.state.Items);

				for (var i = 0; i < catalogsFiltered.filter.length; i++)
					if (typeof e.state[catalogsFiltered.filter[i]] != "undefined") 
						$(this.parent + ' #' + catalogsFiltered.filter[i]).val(e.state[catalogsFiltered.filter[i]]).trigger("chosen:updated");
				$('select').trigger("chosen:updated");

				catalogsFiltered.initPager();
				if (catalogsFiltered.callback != null)
					catalogsFiltered.callback();
			}
		};
	},
	initPager: function () {
		$(".adminPager a").unbind('click').click(function () {
			catalogsFiltered.get({
				url: catalogsFiltered.url,// + $('#getadd').val(),
				data: {
					pageIndex: catalogsFiltered.getIndex(this),
					pageSize: $(".adminPager select").val(),
					search: $("input.finderCatalog").val().toString(),
					select: catalogsFiltered.getSelectVal()
				}
			});
			return false;
		});
		$(".adminPager select").not(".ex").chosen();
		$(".adminPager select").removeAttr("onchange");
		$(".adminPager select").change(function () {
			catalogsFiltered.get({
				url: catalogsFiltered.url,// + $('#getadd').val(),
				data: {
					pageIndex: 1,
					pageSize: $(".adminPager select").val(),
					search: $("input.finderCatalog").val().toString(),
					select: catalogsFiltered.getSelectVal()
				}
			});
		});

		$('.sorting-name').click(function () {
			$('#getadd').val('&SortField=' + $(this).data('sortfield') + '&SortOrder=' + $(this).data('sortorder'));
			catalogsFiltered.get({
				url: catalogsFiltered.url,// + $('#getadd').val(),
				data: {
					pageIndex: 1,
					pageSize: $(".adminPager select").val(),
					search: $('input.finderCatalog').val().toString(),
					select: catalogsFiltered.getSelectVal()
				}
			});
		});

		if (typeof (this.callback) == 'function') {
			this.callback();
		}
	},
	getSelectVal: function () {
		if (!$("select").is("#searchSelect"))
			return 0;
		return $("#searchSelect").val().toString();
	},
	getIndex: function (e) {
		var pIndex = parseInt($("#pagerIndex").val());
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
			pIndex = parseInt($("#pagerCount").val());
		}
		else
			pIndex = parseInt($(e).text());
		return pIndex;
	},
	setData: function (data) {
		for (var i = 0; i < this.filter.length; i++) {
			if ($(this.parent + ' #' + this.filter[i]).attr('multiple') == 'multiple') {
				data[this.filter[i] + "String"] = JSON.stringify($(this.parent + ' #' + this.filter[i]).val());
			}
			else if ($(this.parent + ' #' + this.filter[i]).attr('type') == 'checkbox') {
				data[this.filter[i]] = $(this.parent + ' #' + this.filter[i]).prop("checked");
			}
			else {
				data[this.filter[i]] = $(this.parent + ' #' + this.filter[i]).val();
			}
		}
		this.v = (Math.random() + '').substr(2);
		data['v'] = this.v;
		console.log('catalogsFiltered.v=' + catalogsFiltered.v);
		return data;
	},
	setHistory: function (data, title, url) {
		if (catalogsFiltered.history) {
			for (var i = 0; i < this.filter.length; i++)
				data[this.filter[i]] = $(this.parent + ' #' + this.filter[i]).val();
			window.history.pushState(data, title, url);
		}
	},
	getrun: function () {
		catalogsFiltered.get({
			url: catalogsFiltered.url + ($("#getadd").length ? $('#getadd').val() : ""),
			data: {
				pageIndex: 1,
				pageSize: 10,
				search: $('input.finderCatalog').val().toString(),
				select: catalogsFiltered.getSelectVal()
			}
		});
	},
	getRerun: function () {
		catalogsFiltered.get({
			url: catalogsFiltered.url + ($("#getadd").length ? $('#getadd').val() : ""),
			data: {
				pageIndex: parseInt($("#pagerIndex").val()),
				pageSize: $(".adminPager select").val(),
				search: $('input.finderCatalog').val().toString(),
				select: catalogsFiltered.getSelectVal()
			}
		});
	},
	get: function (sData) {
		var data = catalogsFiltered.setData(sData.data);
		var url = sData.url + ($("#getadd").length ? $('#getadd').val() : "");
		
		$.ajax({
			type: "POST",
			url: url.toLowerCase(),
			data: data,
			success: function (d) {
				var urlajax = (this.url + "&" + this.data).toLowerCase();
				var v = catalogsFiltered.getParameterByName('v', urlajax);
				if (v == '' || v == catalogsFiltered.v) {

					$(".adminPager").html(d.pager);
					$(".adminItems").html(d.items);
					
					catalogsFiltered.setHistory({ "pager": d.pager, Items: d.items }, "", urlajax.replace('type=ajax', 'type='));
					catalogsFiltered.initPager();
					if (catalogsFiltered.callback != null)
						catalogsFiltered.callback();
				}
				else {
					console.log('v=' + v);
				}
			}
		});
	},
	getParameterByName: function (name, url) {
		if (!url) url = window.location.href;
		name = name.replace(/[\[\]]/g, "\\$&");
		var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
		results = regex.exec(url);
		if (!results) return null;
		if (!results[2]) return '';
		return decodeURIComponent(results[2].replace(/\+/g, " "));
	}
};