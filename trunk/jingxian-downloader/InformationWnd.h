/***************************************************************
 * Name:      InformationWnd.h
 * Purpose:   Defines InformationWnd Class
 * Author:    runner.mei (runner.mei@gmail.com)
 * Created:   2008-11-15
 * Copyright: runner.mei
 * License:
 **************************************************************/

#ifndef InformationWnd_H
#define InformationWnd_H


#include <wx/gdicmn.h>
#include <wx/font.h>
#include <wx/colour.h>
#include <wx/settings.h>
#include <wx/string.h>
#include <wx/bitmap.h>
#include <wx/image.h>
#include <wx/icon.h>
#include <wx/treectrl.h>
#include <wx/sizer.h>
#include <wx/panel.h>
#include <wx/listctrl.h>
#include <wx/splitter.h>
#include <wx/aui/auibook.h>
#include <wx/frame.h>


class InformationWnd : public wxAuiNotebook
{
public:
	InformationWnd(wxWindow *parent,
			wxWindowID winid = wxID_ANY,
            const wxPoint& pos = wxDefaultPosition,
			const wxSize& size = wxDefaultSize,
            long style = wxTAB_TRAVERSAL | wxNO_BORDER );

	~InformationWnd(void);

protected:
	wxWindow* createLogPanel( wxAuiNotebook* downloadBook );
	wxWindow* createGraphPanel( wxAuiNotebook* downloadBook );

	void on_threads_right_click( wxTreeEvent& e );
	void on_threads_select_changed( wxTreeEvent& e );
	void on_splitterOnIdle( wxIdleEvent& e);
	
private:
	
	wxTreeCtrl* m_threads;
	wxListCtrl* m_logger;
};

#endif // InformationWnd_H
