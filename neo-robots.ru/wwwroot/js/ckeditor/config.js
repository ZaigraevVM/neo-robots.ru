/**
 * @license Copyright (c) 2003-2012, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.html or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
	// Define changes to default configuration here. For example:
	config.plugins = 'ispersons,isorganizations,iscomparetexts,dialogui,dialog,about,a11yhelp,dialogadvtab,basicstyles,bidi,blockquote,clipboard,button,panelbutton,panel,floatpanel,colorbutton,colordialog,templates,menu,contextmenu,div,resize,toolbar,elementspath,list,indent,enterkey,entities,popup,filebrowser,find,fakeobjects,flash,floatingspace,listblock,richcombo,font,forms,format,htmlwriter,horizontalrule,iframe,wysiwygarea,image,smiley,justify,link,liststyle,magicline,maximize,newpage,pagebreak,pastetext,pastefromword,preview,print,removeformat,save,selectall,showblocks,showborders,sourcearea,specialchar,menubutton,scayt,stylescombo,tab,table,tabletools,undo,wsc';
	config.language = 'ru';
	config.removePlugins = 'pastetext';
	config.forcePasteAsPlainText = true;

	config.resize_enabled = true;
	config.resize_maxWidth = 2500;
	config.resize_maxHeight = 2500;
	config.resize_dir = 'both';

	config.disableNativeSpellChecker = false;
	config.scayt_autoStartup = false;
};