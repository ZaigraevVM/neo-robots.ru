ISCompareTextsForCKE = {

	Settings: { T1: [], T2: [] },

	TextToLines: function (t) {
		var tsp = t.replace('\r', '').split('\n');
		var a = [];
		for (var i = 0; i < tsp.length; i++) {
			var tr = $.trim(tsp[i]);
			if (tr != '')
				a.push(tr);
		}
		return a;
	},

	CheckChangeStrings: function (s1, s2) {
		if (s1 == null || s2 == null || s1 == '' || s2 == '')
			return false;

		var d1 = 0, d2 = 0;

		s1 = s1.replace('line-height: 1.6em;', '').replace(/[^А-ЯЁа-яёA-Za-z0-9_]/gi, " ").replace(/\s+/gi, " ");
		s2 = s2.replace('line-height: 1.6em;', '').replace(/[^А-ЯЁа-яёA-Za-z0-9_]/gi, " ").replace(/\s+/gi, " ");

		var sA1 = s1.split(' ');
		var sA2 = s2.split(' ');

		var sC1 = [];
		for (var i = 0; i < sA1.length; i++) {
			var s = sA1[i].toLowerCase();
			if (s != '' && s != 'p' && s != 'align' && s != 'justify' && s != 'span' && s != 'style' && s != 'line' && s != 'height')
				sC1.push(sA1[i]);
		}

		var sC2 = [];
		for (var i = 0; i < sA2.length; i++) {
			var s = sA2[i].toLowerCase();
			if (s != '' && s != 'p' && s != 'align' && s != 'justify' && s != 'span' && s != 'style' && s != 'line' && s != 'height')
				sC2.push(sA2[i]);
		}

		if (sC1.length < 1 || sC2.length < 1)
			return false;

		for (var i = 0; i < sC1.length; i++)
			if (sC2.indexOf(sC1[i]) > -1)
				d1++;

		for (var i = 0; i < sC2.length; i++)
			if (sC1.indexOf(sC2[i]) > -1)
				d2++;

		//return d1 / sC1.length > 0.7 && d2 / sC2.length > 0.7;

		var maxCompare = d1 / sC1.length > d2 / sC2.length ? d1 / sC1.length : d2 / sC2.length;

		return maxCompare > 0.5;
	},

	HtmlEscape: function (str) { return String(str).replace(/&/g, '&amp;').replace(/"/g, '&quot;').replace(/'/g, '&#39;').replace(/</g, '&lt;').replace(/>/g, '&gt;'); },

	Compare: function (text1, text2) {

		this.Settings.T1 = this.TextToLines(text1);
		this.Settings.T2 = this.TextToLines(text2);

		var mapDuplicate = this.SearchOfLinesByRules(0, 0, this.Settings.T1.length, this.Settings.T2.length, this.CheckCopy);

		var Result = [], i1 = 0, i2 = 0, n1 = 0, n2 = 0, run = true;

		if (mapDuplicate.Map1.length > 0) {
			var ind1 = 0;
			var ind2 = 0;
			for (var i = 0; i < mapDuplicate.Map1.length; i++) {
				ind1 = mapDuplicate.Map1[i];
				ind2 = mapDuplicate.Map2[i];

				var mapCL = { Map1: [], Map2: [] };
				if (i1 < ind1 && i2 < ind2)
					var mapCL = this.SearchOfLinesByRules(i1, i2, ind1, ind2, this.CheckChangeStrings);

				var run = true;
				var ch = 0;
				while (run) {
					ch++;
					if (i1 < ind1 && i2 < ind2) {
						if (mapCL.Map1.length > 0) {
							var imcl1 = mapCL.Map1.indexOf(i1);
							var imcl2 = mapCL.Map2.indexOf(i2);
							if (imcl1 > -1) {
								if (imcl1 == imcl2) {
									var marker = this.MarkerLine(this.Settings.T1[i1], this.Settings.T2[i2]);
									//console.log(marker);
									var p = { n1: i1, l1: marker.m1, s1: 1, n2: i2, l2: marker.m2, s2: 1 };
									
									Result.push(p);
									i1++;
									i2++;
								}
								else if (imcl1 > imcl2) {
									var p = { n1: ' ', l1: '<span class="emptyfull">' + this.HtmlEscape(this.Settings.T2[i2]) + '</span>', s1: 0, n2: i2, l2: this.HtmlEscape(this.Settings.T2[i2]), s2: 3 };
									Result.push(p);
									i2++;
								}
							}
							else {
								var p = { n1: i1, l1: this.HtmlEscape(this.Settings.T1[i1]), s1: 2, n2: ' ', l2: '<span class="emptyfull">' + this.HtmlEscape(this.Settings.T1[i1]) + '</span>', s2: 0 };
								Result.push(p);
								i1++;
							}
						}
						else {
							var p = { n1: ' ', l1: '<span class="emptyfull">' + this.HtmlEscape(this.Settings.T2[i2]) + '</span>', s1: 0, n2: i2, l2: this.HtmlEscape(this.Settings.T2[i2]), s2: 3 };
							Result.push(p);
							i2++;
						}
					}
					else if (i1 < ind1) {
						var p = { n1: i1, l1: this.HtmlEscape(this.Settings.T1[i1]), s1: 2, n2: ' ', l2: '<span class="emptyfull">' + this.HtmlEscape(this.Settings.T1[i1]) + '</span>', s2: 0 };
						Result.push(p);
						i1++;
					}
					else if (i2 < ind2) {
						var p = { n1: ' ', l1: '<span class="emptyfull">' + this.HtmlEscape(this.Settings.T2[i2]) + '</span>', s1: 0, n2: i2, l2: this.HtmlEscape(this.Settings.T2[i2]), s2: 3 };
						Result.push(p);
						i2++;
					}
					else if (i1 == ind1 && i2 == ind2) {
						var p = { n1: i1, l1: this.HtmlEscape(this.Settings.T1[i1]), s1: 0, n2: i2, l2: this.HtmlEscape(this.Settings.T2[i2]), s2: 0 };
						Result.push(p);
						i1++;
						i2++;
					}
					else if ((i1 > ind1 && i2 > ind2) || (i1 >= this.Settings.T1.length && i2 >= this.Settings.T2.length)) {
						run = false;
					}
					if (ch > 1000)
						run = false;
				}
			}

			ind1 = this.Settings.T1.length;
			ind2 = this.Settings.T2.length;

			if (i1 < ind1 && i2 < ind2) 
				var mapCL = this.SearchOfLinesByRules(i1, i2, ind1, ind2, this.CheckChangeStrings);

			var run = true;
			var ch = 0;
			while (run) {
				ch++;
				if (i1 < ind1 && i2 < ind2) {
					if (mapCL.Map1.length > 0) {
						var imcl1 = mapCL.Map1.indexOf(i1);
						var imcl2 = mapCL.Map2.indexOf(i2);
						if (imcl1 > -1) {
							if (imcl1 == imcl2) {
								var marker = this.MarkerLine(this.Settings.T1[i1], this.Settings.T2[i2]);
								var p = { n1: i1, l1: marker.m1, s1: 1, n2: i2, l2: marker.m2, s2: 1 };
								Result.push(p);
								i1++;
								i2++;
							}
							else if (imcl1 > imcl2) {
								var p = { n1: ' ', l1: '<span class="emptyfull">' + this.HtmlEscape(this.Settings.T2[i2]) + '<span>', s1: 0, n2: i2, l2: this.HtmlEscape(this.Settings.T2[i2]), s2: 3 };
								Result.push(p);
								i2++;
							}
							else
							{
								var p = { n1: i1, l1: this.HtmlEscape(this.Settings.T1[i1]), s1: 2, n2: ' ', l2: '<span class="emptyfull">' + this.HtmlEscape(this.Settings.T1[i1]) + '<span>', s2: 0 };
								Result.push(p);
								i1++;
							}
						}
						else {
							var p = { n1: i1, l1: this.HtmlEscape(this.Settings.T1[i1]), s1: 2, n2: ' ', l2: '<span class="emptyfull">' + this.HtmlEscape(this.Settings.T1[i1]) + '<span>', s2: 0 };
							Result.push(p);
							i1++;
						}
					}
					else {
						var p = { n1: ' ', l1: '<span class="emptyfull">' + this.HtmlEscape(this.Settings.T2[i2]) + '<span>', s1: 0, n2: i2, l2: this.HtmlEscape(this.Settings.T2[i2]), s2: 3 };
						Result.push(p);
						i2++;
					}
				}
				else if (i1 < ind1) {
					var p = { n1: i1, l1: this.HtmlEscape(this.Settings.T1[i1]), s1: 2, n2: ' ', l2: '<span class="emptyfull">' + this.HtmlEscape(this.Settings.T1[i1]) + '<span>', s2: 0 };
					Result.push(p);
					i1++;
				}
				else if (i2 < ind2) {
					var p = { n1: ' ', l1: '<span class="emptyfull">' + this.HtmlEscape(this.Settings.T2[i2]) + '<span>', s1: 0, n2: i2, l2: this.HtmlEscape(this.Settings.T2[i2]), s2: 3 };
					Result.push(p);
					i2++;
				}
				else if ((i1 >= ind1 && i2 >= ind2) || (i1 >= this.Settings.T1.length && i2 >= this.Settings.T2.length)) {
					run = false;
				}
				if (ch > 1000)
					run = false;
			}
		}
		else {

			for (var i = 0; i < this.Settings.T1.length; i++) {
				Result.push({ n1: i, l1: this.HtmlEscape(this.Settings.T1[i]), s1: 2, n2: '', l2: '<span class="emptyfull">' + this.HtmlEscape(this.Settings.T1[i]) + '</span>', s2: 0 });
			}

			for (var i = 0; i < this.Settings.T2.length; i++)
				Result.push({ n1: '', l1: '<span class="emptyfull">' + this.HtmlEscape(this.Settings.T2[i]) + '</span>', s1: 0, n2: i, l2: this.HtmlEscape(this.Settings.T2[i]), s2: 3 });
		}

		return Result;
	},

	MarkerLine: function (s1, s2) {
		var l1 = this.LineToWords(s1);
		var l2 = this.LineToWords(s2);

		var l1clear = [], l1clearInd = [], l2clear = [], l2clearInd = [];

		for (var i = 0; i < l1.length; i++)
			if (l1[i].length > 2)
			{
				l1clear.push(l1[i]);
				l1clearInd.push(i);
			}

		for (var i = 0; i < l2.length; i++)
			if (l2[i].length > 2) {
				l2clear.push(l2[i]);
				l2clearInd.push(i);
			}

		var MaxResult = this.SearchOfLinesByRulesMaxRun({ Map1: [], Map2: [], MapMax: [], Rule: this.CheckCopy, Stop: 0, JInd: {}, l1: l1clear, l2: l2clear });
		var map1 = MaxResult.map1, map2 = MaxResult.map2;
		
		var map1clear = [], map2clear = [];

		for (var i = 0; i < map1.length; i++)
		{
			map1clear.push(l1clearInd[map1[i]]);
			map2clear.push(l2clearInd[map2[i]]);
		}


		var m1 = '', m2 = '';
		if (map1clear.length > 0) {
			var i1 = 0, i2 = 0;
			for (var i = 0; i < map1clear.length; i++) {
				var ind1 = map1clear[i];
				var ind2 = map2clear[i];

				//console.log('[[[' + i + ']]]');

				var run = true, ch = 0, l1mini = [], l2mini = [];
				
				while (run) {
					//console.log('i1=' + i1 + '   i2=' + i2);
					ch++;
					if (i1 < ind1 && i2 < ind2) {
						//m1 += '<span class="addword">' + this.HtmlEscape(l1[i1]) + '</span>';
						//m2 += '<span class="deleteword">' + this.HtmlEscape(l2[i2]) + '</span>';
						l1mini.push(l1[i1]);
						l2mini.push(l2[i2]);
						//console.log('[1] i1=' + i1 + '  ind1=' + ind1 + '  i2=' + i2 + '  ind2=' + ind2 + '  this.HtmlEscape(l1[i1])=' + this.HtmlEscape(l1[i1]) + '  this.HtmlEscape(l2[i2])=' + this.HtmlEscape(l2[i2]));

						i1++;
						i2++;
					}
					else if (i1 < ind1) {
						//m1 += '<span class="addword">' + this.HtmlEscape(l1[i1]) + '</span>';

						l1mini.push(l1[i1]);
						//console.log('[2] i1=' + i1 + '  ind1=' + ind1 + '  this.HtmlEscape(l1[i1])=' + this.HtmlEscape(l1[i1]));

						i1++;
					}
					else if (i2 < ind2) {
						//m2 += '<span class="deleteword">' + this.HtmlEscape(l2[i2]) + '</span>';

						l2mini.push(l2[i2]);
						///console.log('[3] i1=' + i1 + '  ind1=' + ind1 + '  i2=' + i2 + '  ind2=' + ind2 + '  this.HtmlEscape(l2[i2])=' + this.HtmlEscape(l2[i2]));

						i2++;
					}
					else if (i1 == ind1 && i2 == ind2) {

						if (l1mini.length > 0 && l2mini.length > 0)
						{
							/*
							console.log('l1mini, l2mini: ');
							console.log(l1mini);
							console.log(l2mini);
							*/

							var solbrmts = this.SearchOfLinesByRulesMaxRunToString({
								Map1: [], Map2: [], MapMax: [], Rule: this.CheckCopy, Stop: 0, JInd: {}, l1: l1mini, l2: l2mini
							});

							//console.log(solbrmts);

							m1 += solbrmts.m1;
							m2 += solbrmts.m2;
						}
						else if (l1mini.length > 0)
						{
							m1 += '<span class="addword">' + this.HtmlEscape(l1mini.join('')) + '</span>';
						}
						else if (l2mini.length > 0)
						{
							m2 += '<span class="deleteword">' + this.HtmlEscape(l2mini.join('')) + '</span>';
						}

						m1 += this.HtmlEscape(l1[i1]);
						m2 += this.HtmlEscape(l2[i2]);

						i1++;
						i2++;
					}
					else if ((i1 > ind1 && i2 > ind2) || (i1 >= l1.length && i2 >= l2.length)) {
						run = false;
					}

					if (ch > 1000)
						run = false;
				}
			}

			/*
			console.log('!!!m1:');
			console.log(m1);

			console.log('!!!m2:');
			console.log(m2);
			*/

			var ind1 = l1.length - 1;
			var ind2 = l2.length - 1;

				var run = true, ch = 0, l1mini = [], l2mini = [];
				
				while (run) {
					//console.log('i1=' + i1 + '   i2=' + i2);
					ch++;
					if (i1 < ind1 && i2 < ind2) {
						//m1 += '<span class="addword">' + this.HtmlEscape(l1[i1]) + '</span>';
						//m2 += '<span class="deleteword">' + this.HtmlEscape(l2[i2]) + '</span>';
						l1mini.push(l1[i1]);
						l2mini.push(l2[i2]);
						//console.log('[1] i1=' + i1 + '  ind1=' + ind1 + '  i2=' + i2 + '  ind2=' + ind2 + '  this.HtmlEscape(l1[i1])=' + this.HtmlEscape(l1[i1]) + '  this.HtmlEscape(l2[i2])=' + this.HtmlEscape(l2[i2]));

						i1++;
						i2++;
					}
					else if (i1 < ind1) {
						//m1 += '<span class="addword">' + this.HtmlEscape(l1[i1]) + '</span>';

						l1mini.push(l1[i1]);
						//console.log('[2] i1=' + i1 + '  ind1=' + ind1 + '  this.HtmlEscape(l1[i1])=' + this.HtmlEscape(l1[i1]));

						i1++;
					}
					else if (i2 < ind2) {
						//m2 += '<span class="deleteword">' + this.HtmlEscape(l2[i2]) + '</span>';

						l2mini.push(l2[i2]);
						///console.log('[3] i1=' + i1 + '  ind1=' + ind1 + '  i2=' + i2 + '  ind2=' + ind2 + '  this.HtmlEscape(l2[i2])=' + this.HtmlEscape(l2[i2]));

						i2++;
					}
					else if (i1 == ind1 && i2 == ind2) {

						if (l1mini.length > 0 && l2mini.length > 0)
						{
							var solbrmts = this.SearchOfLinesByRulesMaxRunToString({
								Map1: [], Map2: [], MapMax: [], Rule: this.CheckCopy, Stop: 0, JInd: {}, l1: l1mini, l2: l2mini
							});

							m1 += solbrmts.m1;
							m2 += solbrmts.m2;
						}
						else if (l1mini.length > 0)
						{
							m1 += '<span class="addword">' + this.HtmlEscape(l1mini.join('')) + '</span>';
							m2 += '<span class="empty">' + this.HtmlEscape(l1mini.join('')) + '</span>';
						}
						else if (l2mini.length > 0)
						{
							m2 += '<span class="deleteword">' + this.HtmlEscape(l2mini.join('')) + '</span>';
							m1 += '<span class="empty">' + this.HtmlEscape(l2mini.join('')) + '</span>';
						}

						m1 += this.HtmlEscape(l1[i1]);
						m2 += this.HtmlEscape(l2[i2]);

						i1++;
						i2++;
					}
					else if ((i1 > ind1 && i2 > ind2) || (i1 >= l1.length && i2 >= l2.length)) {
						run = false;
					}

					if (ch > 1000)
						run = false;
				}
		}
		else
		{
			m1 = s1;
			m2 = s2;
		}

		/*
		console.log('s1:');
		console.log(s1);

		console.log('s2:');
		console.log(s2);

		console.log('m1:');
		console.log(m1);

		console.log('m2:');
		console.log(m2);
		*/

		return { m1: m1, m2: m2 };
	},

	LineToWords: function (s) {
		if (s == null || s.length == 0)
			return [];

		var l = [];
		var letters = 'абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890';
		var i = 0, word = '', type = '';

		while (i < s.length)
		{
			if (letters.indexOf(s[i]) > -1)
				word += s[i];
			else
			{
				if (word.length > 0) l.push(word);
				l.push(s[i]);
				word = '';
			}
			i++;
		}

		if (word.length > 0) l.push(word);

		return l;
	},

	CheckCopy: function (c1, c2) { return c1 == c2; },

	SearchOfLinesByRulesSettings: { Map1: [], Map2: [], MapMax: [], Rule: null, Stop: 0, JInd: {} },

	SearchOfLinesByRules: function (i1, i2, ind1, ind2, rule) {
		this.SearchOfLinesByRulesSettings = { Map1: [], Map2: [], MapMax: [], Rule: rule, Stop: 0, JInd: {} };

		for (var i = i1; i < ind1; i++)
			for (var j = i2; j < ind2; j++) {
				if (this.SearchOfLinesByRulesSettings.Rule(this.Settings.T1[i], this.Settings.T2[j])) {
					this.SearchOfLinesByRulesSettings.Map1.push(i);
					this.SearchOfLinesByRulesSettings.Map2.push(j);
					break;
				}
			}

		if (this.SearchOfLinesByRulesSettings.Map2.length > 0) {
			this.SearchOfLinesByRulesMax([], 0, {});

			if (this.SearchOfLinesByRulesSettings.MapMax.length > 0) {
				var m1 = [], m2 = [];
				for (var i = 0; i < this.SearchOfLinesByRulesSettings.MapMax.length; i++) {
					m1.push(this.SearchOfLinesByRulesSettings.Map1[this.SearchOfLinesByRulesSettings.MapMax[i]]);
					m2.push(this.SearchOfLinesByRulesSettings.Map2[this.SearchOfLinesByRulesSettings.MapMax[i]]);
				}

				return { Map1: m1, Map2: m2 };
			}
		}

		return { Map1: [], Map2: [] };
	},

	SearchOfLinesByRulesMaxRun: function (settings) {
		var map1 = [], map2 = [];
		this.SearchOfLinesByRulesSettings = settings;//{ Map1: [], Map2: [], MapMax: [], Rule: this.CheckCopy, Stop: 0, JInd: {} };

		for (var i = 0; i < settings.l1.length; i++) {
			var d = [];
			for (var j = 0; j < settings.l2.length; j++)
				if (settings.l1[i] == settings.l2[j])
					d.push(j);

			if (d.length == 1) {
				this.SearchOfLinesByRulesSettings.Map1.push(i);
				this.SearchOfLinesByRulesSettings.Map2.push(d[0]);
			}
			else if (d.length > 1) {
				this.SearchOfLinesByRulesSettings.Map1.push(i);
				this.SearchOfLinesByRulesSettings.Map2.push(d);
			}
		}
		if (this.SearchOfLinesByRulesSettings.Map2.length > 0) {
			this.SearchOfLinesByRulesMax([], 0, {});

			if (this.SearchOfLinesByRulesSettings.MapMax.length > 0) {

				if (this.SearchOfLinesByRulesSettings.print == 1) {
					console.log('this.SearchOfLinesByRulesSettings');
					console.log(this.SearchOfLinesByRulesSettings);
				}

				for (var i = 0; i < this.SearchOfLinesByRulesSettings.MapMax.length; i++) {

					if (this.SearchOfLinesByRulesSettings.print == 1) {
						console.log('i=' + i);
					}

					map1.push(this.SearchOfLinesByRulesSettings.Map1[this.SearchOfLinesByRulesSettings.MapMax[i]]);
					if ($.isArray(this.SearchOfLinesByRulesSettings.Map2[this.SearchOfLinesByRulesSettings.MapMax[i]])) {
						map2.push(this.SearchOfLinesByRulesSettings.Map2[this.SearchOfLinesByRulesSettings.MapMax[i]][this.SearchOfLinesByRulesSettings.JInd['j' + this.SearchOfLinesByRulesSettings.MapMax[i]]]);
					}
					else {
						map2.push(this.SearchOfLinesByRulesSettings.Map2[this.SearchOfLinesByRulesSettings.MapMax[i]]);
					}
				}
			}
		}

		return { map1: map1, map2: map2 };
	},

	SearchOfLinesByRulesMaxRunToString: function (settings) {

		if (settings.l1.indexOf("666") > -1) {
			console.log('settings.l1, settings.l2=');
			console.log(settings.l1);
			console.log(settings.l2);

			//settings.print = 1;
		}
		var MaxResult = this.SearchOfLinesByRulesMaxRun(settings);

		if (settings.l1.indexOf("666") > -1) {
			console.log('MaxResult');
			console.log(MaxResult);
		}

		var map1 = MaxResult.map1, map2 = MaxResult.map2;

		var m1 = '', m2 = '', l1 = settings.l1, l2 = settings.l2;
		if (map1.length > 0) {
			var i1 = 0, i2 = 0;
			for (var i = 0; i < map1.length; i++) {
				var ind1 = map1[i];
				var ind2 = map2[i];

				//console.log('[[[' + i + ']]]');

				var run = true, ch = 0, l1mini = [], l2mini = [];

				while (run) {
					//console.log('i1=' + i1 + '   i2=' + i2);
					ch++;
					if (i1 < ind1 && i2 < ind2) {
						//m1 += '<span class="addword">' + this.HtmlEscape(l1[i1]) + '</span>';
						//m2 += '<span class="deleteword">' + this.HtmlEscape(l2[i2]) + '</span>';
						l1mini.push(l1[i1]);
						l2mini.push(l2[i2]);
						//console.log('[1] i1=' + i1 + '  ind1=' + ind1 + '  i2=' + i2 + '  ind2=' + ind2 + '  this.HtmlEscape(l1[i1])=' + this.HtmlEscape(l1[i1]) + '  this.HtmlEscape(l2[i2])=' + this.HtmlEscape(l2[i2]));

						i1++;
						i2++;
					}
					else if (i1 < ind1) {
						//m1 += '<span class="addword">' + this.HtmlEscape(l1[i1]) + '</span>';

						l1mini.push(l1[i1]);
						//console.log('[2] i1=' + i1 + '  ind1=' + ind1 + '  this.HtmlEscape(l1[i1])=' + this.HtmlEscape(l1[i1]));

						i1++;
					}
					else if (i2 < ind2) {
						//m2 += '<span class="deleteword">' + this.HtmlEscape(l2[i2]) + '</span>';

						l2mini.push(l2[i2]);
						//console.log('[3] i1=' + i1 + '  ind1=' + ind1 + '  i2=' + i2 + '  ind2=' + ind2 + '  this.HtmlEscape(l2[i2])=' + this.HtmlEscape(l2[i2]));

						i2++;
					}
					else if (i1 == ind1 && i2 == ind2) {
						/*
						console.log('------l1mini, l2mini: ');
						console.log(l1mini);
						console.log(l2mini);
						*/
						if (l1mini.length > 0) {
							m1 += '<span class="addword">' + this.HtmlEscape(l1mini.join('')) + '</span>';
							m2 += '<span class="empty">' + this.HtmlEscape(l1mini.join('')) + '</span>';
						}
						if (l2mini.length > 0) {
							m2 += '<span class="deleteword">' + this.HtmlEscape(l2mini.join('')) + '</span>';
							m1 += '<span class="empty">' + this.HtmlEscape(l2mini.join('')) + '</span>';
						}
						m1 += this.HtmlEscape(l1[i1]);
						m2 += this.HtmlEscape(l2[i2]);

						//console.log('[4] i1=' + i1 + '  ind1=' + ind1 + '  i2=' + i2 + '  ind2=' + ind2 + '  this.HtmlEscape(l1[i1])=' + this.HtmlEscape(l1[i1]) + '  this.HtmlEscape(l2[i2])=' + this.HtmlEscape(l2[i2]));

						i1++;
						i2++;
					}
					else if ((i1 > ind1 && i2 > ind2) || (i1 >= l1.length && i2 >= l2.length)) {
						//console.log('[5] i1=' + i1 + '  ind1=' + ind1 + '  i2=' + i2 + '  ind2=' + ind2 + '  l1.length=' + l1.length + '  l2.length=' + l2.length);
						run = false;
					}

					if (ch > 1000)
						run = false;
				}
			}

			var ind1 = l1.length;
			var ind2 = l2.length;

			//console.log('[[[' + i + ']]]');

			/*
			console.log('|||||||||');
			console.log('l1,l2=');
			console.log(l1);
			console.log(l2);
			console.log('i1=' + i1 + ' i2=' + i2 + ' ind1=' + ind1 + ' ind1=' + ind2);
			*/

			var run = true, ch = 0, l1mini = [], l2mini = [];

			while (run) {
				//console.log('i1=' + i1 + '   i2=' + i2);
				ch++;
				//console.log('[0]');
				if (i1 < ind1 && i2 < ind2) {
					//console.log('[1]');
					//m1 += '<span class="addword">' + this.HtmlEscape(l1[i1]) + '</span>';
					//m2 += '<span class="deleteword">' + this.HtmlEscape(l2[i2]) + '</span>';
					l1mini.push(l1[i1]);
					l2mini.push(l2[i2]);
					//console.log('[1] i1=' + i1 + '  ind1=' + ind1 + '  i2=' + i2 + '  ind2=' + ind2 + '  this.HtmlEscape(l1[i1])=' + this.HtmlEscape(l1[i1]) + '  this.HtmlEscape(l2[i2])=' + this.HtmlEscape(l2[i2]));

					i1++;
					i2++;
				}
				else if (i1 < ind1) {
					//console.log('[2]');
					//m1 += '<span class="addword">' + this.HtmlEscape(l1[i1]) + '</span>';

					l1mini.push(l1[i1]);
					//console.log('[2] i1=' + i1 + '  ind1=' + ind1 + '  this.HtmlEscape(l1[i1])=' + this.HtmlEscape(l1[i1]));

					i1++;
				}
				else if (i2 < ind2) {
					//console.log('[3]');
					//m2 += '<span class="deleteword">' + this.HtmlEscape(l2[i2]) + '</span>';

					l2mini.push(l2[i2]);
					//console.log('[3] i1=' + i1 + '  ind1=' + ind1 + '  i2=' + i2 + '  ind2=' + ind2 + '  this.HtmlEscape(l2[i2])=' + this.HtmlEscape(l2[i2]));

					i2++;
				}
				else if ((i1 > ind1 && i2 > ind2) || (i1 >= l1.length && i2 >= l2.length)) {
					//console.log('[5]');

					if (l1mini.length > 0) {
						m1 += '<span class="addword">' + this.HtmlEscape(l1mini.join('')) + '</span>';
						m2 += '<span class="empty">' + this.HtmlEscape(l1mini.join('')) + '</span>';
					}
					if (l2mini.length > 0) {
						m2 += '<span class="deleteword">' + this.HtmlEscape(l2mini.join('')) + '</span>';
						m1 += '<span class="empty">' + this.HtmlEscape(l2mini.join('')) + '</span>';
					}

					run = false;
				}

				//console.log('[6]');

				if (ch > 1000)
					run = false;
			}

			/*
			console.log('l1mini,l1mini=');

			console.log(l1mini);
			console.log(l2mini);

			console.log('!!!!!!!!!!!!!!!!!!!!!!!');
			*/

		}
		else {
			m1 = s1;
			m2 = s2;
		}

		return { m1: m1, m2: m2 };
	},

	SearchOfLinesByRulesMax: function (s, i, jind) {

		var print = false;
		if (this.SearchOfLinesByRulesSettings.print == 1 && s.length == 3 && s[0] == 0 && s[1] == 1 && s[2] == 3) {
			//print = true;
		}

		if ($.isArray(s)) {
			this.SearchOfLinesByRulesSettings.Stop++;
			if (this.SearchOfLinesByRulesSettings.Stop > 10000) {
				//console.log('STOP!!!---' + this.SearchOfLinesByRulesSettings.Stop);
				return 0;
			}

			if (s.length > this.SearchOfLinesByRulesSettings.MapMax.length) {
				this.SearchOfLinesByRulesSettings.MapMax = s.slice();
				this.SearchOfLinesByRulesSettings.JInd = $.extend({}, jind);

				/*
				if (print) {
					console.log('this.SearchOfLinesByRulesSettings.MapMax,    this.SearchOfLinesByRulesSettings.JInd=');
					console.log(this.SearchOfLinesByRulesSettings.MapMax);
					console.log(this.SearchOfLinesByRulesSettings.JInd);
				}
				*/
			}

			if (this.SearchOfLinesByRulesSettings.Map2.length - i + s.length > this.SearchOfLinesByRulesSettings.MapMax.length) {
				for (var j = i; j < this.SearchOfLinesByRulesSettings.Map2.length; j++) {

					if (print)
					{
						console.log('j=' + j);
					}

					if ($.isArray(this.SearchOfLinesByRulesSettings.Map2[j]) && this.SearchOfLinesByRulesSettings.Map2[j].length > 0) {

						if (print) {
							console.log('[0] if ($.isArray(this.SearchOfLinesByRulesSettings.Map2[j]) && this.SearchOfLinesByRulesSettings.Map2[j].length > 0) {');
						}


						var map2jArray = this.SearchOfLinesByRulesSettings.Map2[j];
						for (var map2j_i = 0; map2j_i < map2jArray.length; map2j_i++)
						{
							if (print) {
								console.log('[0] map2j_i=' + map2j_i);
							}

							var jindcopy = $.extend({}, jind);
							jindcopy['j' + j] = map2j_i;

							if (s.length == 0) {
								if (print) {
									console.log('[00] map2j_i=' + map2j_i);
								}

								var scopy = [];
								scopy.push(j);

								/*
								if (print) {
									console.log('if (s.length == 0) { scopy=');
									console.log(scopy);
								}
								*/

								this.SearchOfLinesByRulesMax(scopy, j + 1, jindcopy);
							}
							else {

								if (print) {
									console.log('[01] map2j_i=' + map2j_i);
								}

								if ($.isArray(this.SearchOfLinesByRulesSettings.Map2[s[s.length - 1]]) && this.SearchOfLinesByRulesSettings.Map2[s[s.length - 1]].length > 0) {

									

									var map2prev = this.SearchOfLinesByRulesSettings.Map2[s[s.length - 1]];
									var jprev = s[s.length - 1];

									if (print) {
										console.log('[010] map2j_i=' + map2j_i);

										console.log('jind');
										console.log(jind);
										console.log('j' + jprev);

										console.log('map2prev');
										console.log(map2prev);


										console.log('map2jArray');
										console.log(map2jArray);

										console.log("map2prev[jind['j' + jprev]]=" + map2prev[jind['j' + jprev]]);
										console.log("map2jArray[map2j_i]=" + map2jArray[map2j_i]);
										console.log('---------------------------------------------');
									}
									

									if (map2prev[jind['j' + jprev]] < map2jArray[map2j_i]) {
										var scopy = s.slice();
										scopy.push(j);

										if (print) {
											console.log('[0100] map2j_i=' + map2j_i);
										}

										this.SearchOfLinesByRulesMax(scopy, j + 1, jindcopy);
									}
								}
								else if (this.SearchOfLinesByRulesSettings.Map2[s[s.length - 1]] < map2jArray[map2j_i]) {

									if (print) {
										console.log('[011] map2j_i=' + map2j_i);
									}

									var scopy = s.slice();
									scopy.push(j);

									/*
									if (print) {
										console.log('if (s[s.length - 1] < map2jArray[map2j_i]) { map2jArray = ');
										console.log(map2jArray);

										console.log('scopy:');
										console.log(scopy);
									}
									*/

									this.SearchOfLinesByRulesMax(scopy, j + 1, jindcopy);
								}

								else
								{
									if (print) {
										console.log('[02] if ($.isArray(this.SearchOfLinesByRulesSettings.Map2[j]) && this.SearchOfLinesByRulesSettings.Map2[j].length > 0) {');
									}
								}
							}
						}
					}
					else {

						if (print) {
							console.log('[1] j=' + j);
						}

						if (s.length == 0) {
							var scopy = [];
							scopy.push(j);

							if (print) {
								console.log('[10] else {');
							}

							this.SearchOfLinesByRulesMax(scopy, j + 1, jind);
						}
						else {

							if (print) {
								console.log('[11] else {');

								console.log('s');
								console.log(s);

								console.log('s[' + (s.length - 1) + ']=' + s[s.length - 1]);
								console.log('this.SearchOfLinesByRulesSettings.Map2[' + j + ']=' + this.SearchOfLinesByRulesSettings.Map2[j]);
								

							}

							if ($.isArray(this.SearchOfLinesByRulesSettings.Map2[s[s.length - 1]]) && this.SearchOfLinesByRulesSettings.Map2[s[s.length - 1]].length > 0) {

								var map2prev = this.SearchOfLinesByRulesSettings.Map2[s[s.length - 1]];
								var jprev = s[s.length - 1];

								if (print) {
									console.log('[110]--------------');

									console.log('s');
									console.log(s);

									console.log('i='+i);

									console.log('jind');
									console.log(jind);

									console.log('map2prev');
									console.log(map2prev);

									console.log("map2prev[jind['j'" + jprev + "]]=" + map2prev[jind['j' + jprev]]);
									console.log('this.SearchOfLinesByRulesSettings.Map2[j]=' + this.SearchOfLinesByRulesSettings.Map2[j]);

									console.log('[110]=============');
								}

								if (map2prev[jind['j' + jprev]] < this.SearchOfLinesByRulesSettings.Map2[j])
								{
									if (print) {
										console.log('[1100]');
									}

									var scopy = s.slice();
									scopy.push(j);

									if (print) {
										console.log('______scopy:');
										console.log(scopy);

										console.log('jind=');
										console.log(jind);

										console.log("jind['j' + jprev]=");
										console.log(jind['j' + jprev]);
									}

									this.SearchOfLinesByRulesMax(scopy, j + 1, jind);
								}
							}
							else if (this.SearchOfLinesByRulesSettings.Map2[s[s.length - 1]] < this.SearchOfLinesByRulesSettings.Map2[j]) {

								if (print) {
									console.log('[111] else {');
								}

								var scopy = s.slice();
								scopy.push(j);

								/*
								if (print) {
									console.log('========= scopy:');
									console.log(scopy);
								}
								*/

								this.SearchOfLinesByRulesMax(scopy, j + 1, jind);
							}
							else
							{
								if (print) {
									console.log('[112] else {');
								}
							}
						}
					}
				}
			}
		}
	}
}
