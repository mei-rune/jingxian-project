/***************************************************************
 * Name:      InformationWnd.cpp
 * Purpose:   Code for InformationWnd Class
 * Author:    runner.mei (runner.mei@gmail.com)
 * Created:   2008-11-15
 * Copyright: runner.mei
 * License:
 **************************************************************/

#include "pre_config.h"
#include "InformationWnd.h"

InformationWnd::InformationWnd( wxWindow *parent,
			wxWindowID winid,
            const wxPoint& pos,
			const wxSize& size,
            long style )
			: wxAuiNotebook( parent, winid, pos, size, style)
{
	AddPage( createGraphPanel( this ), _("Graph"), false, wxNullBitmap );
	AddPage( createLogPanel( this ), _("Log"), true, wxNullBitmap );

	m_threads->Connect( wxEVT_COMMAND_TREE_ITEM_RIGHT_CLICK, wxTreeEventHandler( InformationWnd::on_threads_right_click ), NULL, this );
	m_threads->Connect( wxEVT_COMMAND_TREE_SEL_CHANGED, wxTreeEventHandler( InformationWnd::on_threads_select_changed ), NULL, this );
}

InformationWnd::~InformationWnd(void)
{
	m_threads->Disconnect( wxEVT_COMMAND_TREE_ITEM_RIGHT_CLICK, wxTreeEventHandler( InformationWnd::on_threads_right_click ), NULL, this );
	m_threads->Disconnect( wxEVT_COMMAND_TREE_SEL_CHANGED, wxTreeEventHandler( InformationWnd::on_threads_select_changed ), NULL, this );
}

wxWindow* InformationWnd::createGraphPanel( wxAuiNotebook* downloadBook )
{
	wxPanel* graphPanel = new wxPanel( downloadBook, wxID_ANY, wxDefaultPosition, wxDefaultSize, wxTAB_TRAVERSAL );
	//wxBoxSizer* graphSizer = new wxBoxSizer( wxVERTICAL );
	//graphPanel->SetSizer( graphSizer );
	//graphPanel->Layout();
	//graphSizer->Fit( graphPanel );
	return graphPanel;
}

wxWindow* InformationWnd::createLogPanel( wxAuiNotebook* downloadBook )
{
	
	wxSplitterWindow* logSplitter = new wxSplitterWindow( downloadBook, wxID_ANY, wxDefaultPosition, wxDefaultSize, wxSP_3D );
	logSplitter->Connect( wxEVT_IDLE, wxIdleEventHandler( InformationWnd::on_splitterOnIdle ), NULL, this );
	
	m_threads = new wxTreeCtrl( logSplitter, wxID_ANY, wxDefaultPosition, wxDefaultSize, 0|wxNO_BORDER );
	m_logger = new wxListCtrl( logSplitter, wxID_ANY, wxDefaultPosition, wxDefaultSize, wxLC_REPORT|wxLC_SINGLE_SEL );
	m_logger->InsertColumn(0, _("Date") );
	m_logger->InsertColumn(1, _("Information"), wxLIST_FORMAT_LEFT, 300 );

	logSplitter->SplitVertically( m_threads, m_logger, 100 );
	return logSplitter;
}

void InformationWnd::on_splitterOnIdle( wxIdleEvent& e)
{
	 wxSplitterWindow *splitter = wxDynamicCast(e.GetEventObject(), wxSplitterWindow);
	if( NULL !=  splitter)
	{
        splitter->SetSashPosition( 200 );
        splitter->Disconnect( wxEVT_IDLE, wxIdleEventHandler( InformationWnd::on_splitterOnIdle ), NULL, this );
	}
}


void InformationWnd::on_threads_right_click( wxTreeEvent& e )
{
	e.Skip(); 
}

void InformationWnd::on_threads_select_changed( wxTreeEvent& e )
{
	e.Skip(); 
}