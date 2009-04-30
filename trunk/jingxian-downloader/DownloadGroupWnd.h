/***************************************************************
 * Name:      DownloadTaskList.h
 * Purpose:   Defines DownloadTaskList Class
 * Author:    runner.mei (runner.mei@gmail.com)
 * Created:   2008-11-15
 * Copyright: runner.mei
 * License:
 **************************************************************/

#ifndef DOWNLOADGROUPWND_H
#define DOWNLOADGROUPWND_H


#include <wx/gdicmn.h>
#include <wx/font.h>
#include <wx/colour.h>
#include <wx/settings.h>
#include <wx/string.h>
#include <wx/bitmap.h>
#include <wx/image.h>
#include <wx/icon.h>
#include <wx/sizer.h>
#include <wx/treectrl.h>
#include <wx/panel.h>
#include <wx/frame.h>


class DownloadGroupWnd : public wxTreeCtrl
{
public:

	DownloadGroupWnd(wxWindow *parent, wxWindowID id = wxID_ANY,
               const wxPoint& pos = wxDefaultPosition,
               const wxSize& size = wxDefaultSize,
               long style = wxTR_HAS_BUTTONS | wxTR_LINES_AT_ROOT,
               const wxValidator& validator = wxDefaultValidator,
               const wxString& name = wxTreeCtrlNameStr);

	~DownloadGroupWnd(void);

protected:
	
	void on_category_right_click( wxTreeEvent& e );
	void on_category_select_changed( wxTreeEvent& e );

private:

};

#endif // DOWNLOADGROUPWND_H