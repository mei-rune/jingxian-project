/***************************************************************
 * Name:      DownloadTaskList.cpp
 * Purpose:   Code for DownloadTaskList Class
 * Author:    runner.mei (runner.mei@gmail.com)
 * Created:   2008-11-15
 * Copyright: runner.mei
 * License:
 **************************************************************/

#include "pre_config.h"
#include "DownloadTaskList.h"

DownloadTaskList::DownloadTaskList( wxWindow *parent,
            wxWindowID winid,
            const wxPoint& pos,
			const wxSize& size,
            long style,
            const wxString& name )
			: wxPanel( parent, winid, pos, size, style, name)
{
	m_download_list = new wxListCtrl( this, wxID_ANY, wxDefaultPosition, wxSize( -1, 200 ), wxLC_REPORT|wxLC_SINGLE_SEL );

	m_download_list->InsertColumn(0, wxT("") , wxLIST_FORMAT_CENTRE, 30 );
 	m_download_list->InsertColumn(1, wxT("Name"), wxLIST_FORMAT_LEFT, 150 );
    m_download_list->InsertColumn(2, wxT("Size") );
    m_download_list->InsertColumn(3, wxT("Completed") );
    m_download_list->InsertColumn(4, wxT("Percent") );
    m_download_list->InsertColumn(5, wxT("Elapse") );
    m_download_list->InsertColumn(6, wxT("Left") );
    m_download_list->InsertColumn(7, wxT("Speed") );
    m_download_list->InsertColumn(8, wxT("Num") );
    m_download_list->InsertColumn(9, wxT("URL") , wxLIST_FORMAT_LEFT, 300 );
    m_download_list->InsertColumn(10, wxT("Comment") , wxLIST_FORMAT_LEFT, 200 );

	wxBoxSizer* downloadListSizer = new wxBoxSizer( wxVERTICAL );
	downloadListSizer->Add( m_download_list, 1, wxALL|wxEXPAND, 0 );
	this->SetSizer( downloadListSizer );
	this->Layout();
	downloadListSizer->Fit( this );

	m_download_list->Connect( wxEVT_COMMAND_LIST_ITEM_SELECTED, wxListEventHandler( DownloadTaskList::on_download_list_select_changed ), NULL, this );
}

DownloadTaskList::~DownloadTaskList(void)
{
	
	m_download_list->Disconnect( wxEVT_COMMAND_LIST_ITEM_SELECTED, wxListEventHandler( DownloadTaskList::on_download_list_select_changed ), NULL, this );
}



void DownloadTaskList::on_download_list_select_changed( wxListEvent& e )
{
	e.Skip(); 
}