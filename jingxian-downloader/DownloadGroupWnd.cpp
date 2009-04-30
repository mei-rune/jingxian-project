/***************************************************************
 * Name:      DownloadTaskList.cpp
 * Purpose:   Code for DownloadGroupWnd Class
 * Author:    runner.mei (runner.mei@gmail.com)
 * Created:   2008-11-15
 * Copyright: runner.mei
 * License:
 **************************************************************/

#include "pre_config.h"
#include "icons/downloadGroupImageList.xpm"
#include "icons/jingxian_16.xpm"
#include "DownloadGroupWnd.h"

DownloadGroupWnd::DownloadGroupWnd(wxWindow *parent, wxWindowID id,
               const wxPoint& pos,
               const wxSize& size,
               long style,
               const wxValidator& validator,
               const wxString& name )
			   : wxTreeCtrl( parent, id, pos, size, style, validator, name)
{
	AssignImageList( new wxImageList(16, 16, true) );
	this->GetImageList()->Add( wxImage( jingxian_16 ) );
	wxImage downloadGroupImage( downloadGroupImageList );

	for(int i = 0; i < 17; i ++ )
	{
		this->GetImageList()->Add( downloadGroupImage.GetSubImage( wxRect( 16*i, 0, 16,16 ) ) );
	}
	
	AddRoot( _("jingxian_downloader"), 0 );
    //wxTreeItemId root = AddRoot( _("jingxian_downloader"), 0 );
    //InsertItem( root, 0, _("downloading"));
	//wxTreeItemId downloaded = InsertItem( root, 1, _("downloaded"));
	//InsertItem( downloaded, 0, _("Music"));

	Connect( wxEVT_COMMAND_TREE_ITEM_RIGHT_CLICK, wxTreeEventHandler( DownloadGroupWnd::on_category_right_click ), NULL, this );
	Connect( wxEVT_COMMAND_TREE_SEL_CHANGED, wxTreeEventHandler( DownloadGroupWnd::on_category_select_changed ), NULL, this );
}

DownloadGroupWnd::~DownloadGroupWnd(void)
{
	Disconnect( wxEVT_COMMAND_TREE_ITEM_RIGHT_CLICK, wxTreeEventHandler( DownloadGroupWnd::on_category_right_click ), NULL, this );
	Disconnect( wxEVT_COMMAND_TREE_SEL_CHANGED, wxTreeEventHandler( DownloadGroupWnd::on_category_select_changed ), NULL, this );

}

void DownloadGroupWnd::on_category_right_click( wxTreeEvent& e )
{
	e.Skip(); 
}

void DownloadGroupWnd::on_category_select_changed( wxTreeEvent& e )
{
	e.Skip(); 
}