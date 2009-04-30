/***************************************************************
 * Name:      DownloadTaskWnd.h
 * Purpose:   Defines DownloadTaskWnd Class
 * Author:    runner.mei (runner.mei@gmail.com)
 * Created:   2008-11-15
 * Copyright: runner.mei
 * License:
 **************************************************************/

#ifndef DOWNLOADWND_H
#define DOWNLOADWND_H

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
#include <wx/splitter.h>
#include <wx/frame.h>
#include "DownloadTaskList.h"
#include "InformationWnd.h"

class DownloadTaskWnd : public wxSplitterWindow
{
public:
	DownloadTaskWnd(wxWindow *parent,
			wxWindowID winid = wxID_ANY,
            const wxPoint& pos = wxDefaultPosition,
			const wxSize& size = wxDefaultSize,
            long style = wxTAB_TRAVERSAL | wxNO_BORDER,
            const wxString& name = wxPanelNameStr);

	~DownloadTaskWnd(void);

protected:

	void on_splitterOnIdle( wxIdleEvent& e);

	
        wxPanel* createTreePanel( wxWindow* mainSplitter );
        wxPanel* createGraphPanel( wxAuiNotebook* downloadBook );
        wxPanel* createLogPanel( wxAuiNotebook* downloadBook );

private:
		DownloadTaskList* m_list;
		InformationWnd* m_logger;
};

#endif // DOWNLOADWND_H