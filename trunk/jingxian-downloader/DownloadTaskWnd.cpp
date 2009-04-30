/***************************************************************
 * Name:      DownloadTaskWnd.cpp
 * Purpose:   Code for DownloadTaskWnd Class
 * Author:    runner.mei (runner.mei@gmail.com)
 * Created:   2008-11-15
 * Copyright: runner.mei
 * License:
 **************************************************************/

#include "pre_config.h"
#include "jx.h"
#include "DownloadTaskWnd.h"

DownloadTaskWnd::DownloadTaskWnd( wxWindow *parent,
			wxWindowID winid,
            const wxPoint& pos,
			const wxSize& size,
            long style,
            const wxString& name )
			: wxSplitterWindow( parent, winid, pos, size, style, name)
{
	Connect( wxEVT_IDLE, wxIdleEventHandler( DownloadTaskWnd::on_splitterOnIdle ), NULL, this );

	m_list= new DownloadTaskList( this);
	m_logger= new InformationWnd( this);
	
	SplitHorizontally( m_list, m_logger, 200 );
}

DownloadTaskWnd::~DownloadTaskWnd(void)
{
}


void DownloadTaskWnd::on_splitterOnIdle( wxIdleEvent& e)
{
	 wxSplitterWindow *splitter = wxDynamicCast(e.GetEventObject(), wxSplitterWindow);
	if( NULL !=  splitter)
	{
		splitter->SetSashPosition( ( int) (splitter->GetSize().GetHeight() * 0.8) );
        splitter->Disconnect( wxEVT_IDLE, wxIdleEventHandler( DownloadTaskWnd::on_splitterOnIdle ), NULL, this );
	}
}