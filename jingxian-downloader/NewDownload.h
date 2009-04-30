///////////////////////////////////////////////////////////////////////////
// C++ code generated with wxFormBuilder (version Apr 16 2008)
// http://www.wxformbuilder.org/
//
// PLEASE DO "NOT" EDIT THIS FILE!
///////////////////////////////////////////////////////////////////////////

#ifndef NEWDOWNLOAD_H
#define NEWDOWNLOAD_H

#include <wx/statusbr.h>
#include <wx/gdicmn.h>
#include <wx/font.h>
#include <wx/colour.h>
#include <wx/settings.h>
#include <wx/string.h>
#include <wx/bitmap.h>
#include <wx/image.h>
#include <wx/icon.h>
#include <wx/menu.h>
#include <wx/toolbar.h>
#include <wx/treectrl.h>
#include <wx/sizer.h>
#include <wx/panel.h>
#include <wx/listctrl.h>
#include <wx/splitter.h>
#include <wx/aui/auibook.h>
#include <wx/frame.h>
#include <wx/dialog.h>
#include <wx/stattext.h>
#include <wx/textctrl.h>
#include <wx/statline.h>
#include <wx/button.h>
#include <wx/checkbox.h>
#include <wx/gbsizer.h>

///////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////////////
/// Class NewDownload
///////////////////////////////////////////////////////////////////////////////
class NewDownload : public wxDialog 
{
	private:
	
	protected:
		wxAuiNotebook* m_option_auinotebook;
		wxPanel* m_general_option_panel;
		wxStaticText* m_staticText5;
		wxTextCtrl* m_textCtrl4;
		wxStaticText* m_staticText6;
		wxTextCtrl* m_textCtrl5;
		wxStaticLine* m_staticline5;
		wxStaticText* m_staticText7;
		wxTextCtrl* m_textCtrl6;
		wxStaticText* m_staticText11;
		wxStaticText* m_staticText9;
		wxTextCtrl* m_textCtrl8;
		wxButton* m_button7;
		wxStaticText* m_staticText10;
		wxTextCtrl* m_textCtrl9;
		wxButton* m_button8;
		wxPanel* m_advencd_option;
		wxCheckBox* m_login_checkBox;
		wxStaticText* m_user_label;
		wxTextCtrl* m_user_name_box;
		wxStaticText* m_password_label;
		wxTextCtrl* m_password_box;
		wxTextCtrl* m_textCtrl12;
		wxStaticText* m_staticText13;
		wxPanel* m_connect_option_panel;
		wxStaticLine* m_staticline17;
		wxButton* m_button59;
		wxButton* m_button60;
	
	public:
		NewDownload( wxWindow* parent, wxWindowID id = wxID_ANY, const wxString& title = wxEmptyString, const wxPoint& pos = wxDefaultPosition, const wxSize& size = wxSize( 589,566 ), long style = wxDEFAULT_DIALOG_STYLE );
		~NewDownload();
	
};

#endif //NEWDOWNLOAD_H
