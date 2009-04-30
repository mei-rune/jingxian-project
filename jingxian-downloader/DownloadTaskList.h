/***************************************************************
 * Name:      DownloadTaskList.h
 * Purpose:   Defines DownloadTaskList Class
 * Author:    runner.mei (runner.mei@gmail.com)
 * Created:   2008-11-15
 * Copyright: runner.mei
 * License:
 **************************************************************/

#ifndef DOWNLOADLIST_H
#define DOWNLOADLIST_H


#include <wx/gdicmn.h>
#include <wx/font.h>
#include <wx/colour.h>
#include <wx/settings.h>
#include <wx/string.h>
#include <wx/bitmap.h>
#include <wx/image.h>
#include <wx/icon.h>
#include <wx/sizer.h>
#include <wx/panel.h>
#include <wx/listctrl.h>
#include <wx/frame.h>


class DownloadTaskList : public wxPanel
{
public:
	DownloadTaskList(wxWindow *parent,
			wxWindowID winid = wxID_ANY,
            const wxPoint& pos = wxDefaultPosition,
			const wxSize& size = wxDefaultSize,
            long style = wxTAB_TRAVERSAL | wxNO_BORDER,
            const wxString& name = wxPanelNameStr);

	~DownloadTaskList(void);

protected:

	void on_download_list_select_changed( wxListEvent& e );

private:
		wxListCtrl* m_download_list;
};

#endif //DOWNLOADLIST_H