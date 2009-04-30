/***************************************************************
 * Name:      jinxian_downloaderMain.cpp
 * Purpose:   Code for Application Frame
 * Author:    runner.mei (runner.mei@gmail.com)
 * Created:   2008-10-31
 * Copyright: runner.mei ()
 * License:
 **************************************************************/

#include "pre_config.h"
#include "DownloadFrame.h"
#include "icons/toolbar_invalid.xpm"
#include "icons/toolbar_enable.xpm"
#include "NewDownload.h"

	//private:
	//
	//	wxBitmap m_bitmap_browse_folder[2];
	//	wxBitmap m_bitmap_delete_download[2];
	//	wxBitmap m_bitmap_download_properties[2];
	//	wxBitmap m_bitmap_find[2];
	//	wxBitmap m_bitmap_find_next[2];
	//	wxBitmap m_bitmap_move_down[2];
	//	wxBitmap m_bitmap_move_up[2];
	//	wxBitmap m_bitmap_new_download[2];
	//	wxBitmap m_bitmap_open_download_file[2];
	//	wxBitmap m_bitmap_options[2];
	//	wxBitmap m_bitmap_pause_download[2];
	//	wxBitmap m_bitmap_start_download[2];

	//	void initBitmap(const wxBitmap& xpm, int index );
	//	void initialize();

void DownloadFrame::initialize()
{

	wxBitmap invalid_bitmap( toolbar_invalid );
	wxBitmap enable_bitmap( toolbar_enable );
	initBitmap( invalid_bitmap, 0 );
	initBitmap( enable_bitmap, 1 );
}

void DownloadFrame::initBitmap(const wxBitmap& xpm, int index )
{
	m_bitmap_browse_folder[index] = xpm.GetSubBitmap( wxRect(5 *24, 0, 24,24 ) );
	m_bitmap_delete_download[index] = xpm.GetSubBitmap( wxRect(3 *24, 0, 24,24 ) );
	m_bitmap_download_properties[index] = xpm.GetSubBitmap( wxRect(6 *24, 0, 24,24 ) );
	m_bitmap_find[index] = xpm.GetSubBitmap( wxRect(11 *24, 0, 24,24 ) );
	m_bitmap_find_next[index] = xpm.GetSubBitmap( wxRect(12 *24, 0, 24,24 ) );
	m_bitmap_move_down[index] = xpm.GetSubBitmap( wxRect(8 *24, 0, 24,24 ) );
	m_bitmap_move_up[index] = xpm.GetSubBitmap( wxRect(7 *24, 0, 24,24 ) );
	m_bitmap_new_download[index] = xpm.GetSubBitmap( wxRect(0 *24, 0, 24,24 ) );
	m_bitmap_open_download_file[index] = xpm.GetSubBitmap( wxRect(4 *24, 0, 24,24 ) );
	m_bitmap_options[index] = xpm.GetSubBitmap( wxRect(9 *24, 0, 24,24 ) );
	m_bitmap_pause_download[index] = xpm.GetSubBitmap( wxRect(2 *24, 0, 24,24 ) );
	m_bitmap_start_download[index] = xpm.GetSubBitmap( wxRect(1 *24, 0, 24,24 ) );
}


void DownloadFrame::initializeCategoryTree()
{
}

void DownloadFrame::initializeDownloadList()
{
}

void DownloadFrame::on_new_download( wxCommandEvent& e )
{
	NewDownload* download = new NewDownload( this );
    download->ShowModal();
}

void DownloadFrame::on_add_batch_dwonload( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_launch_dwonload_file( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_browse_folder( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_check_for_file_update( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_download_again( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_start_download( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_pause_download( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_schedule_download( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_start_all_downloads( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_pause_all_downloads( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_import_download_list( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_import_broken_downloads( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_import_links_from_local_file( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_export_download_list( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_export_download_information( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_exit( wxCommandEvent& e )
{
	Close(); 
}

void DownloadFrame::on_select_all_downloads( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_invert_selection( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_find_download( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_find_next_download( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_move_up( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_move_down( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_delete_download( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_move_to( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_copy_url_to_clipbroad( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_show_detail_panel( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_drop_zone_window( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_show_class_panel( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_export_download_file( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_rename_file( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_rename_commet_as_filename( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_new_category( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_move_category_to( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_delete_category( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_show_category_properties_window( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_new_database( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_open_database( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_merge_database( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_save_database( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_backup_to_database( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_imprt_previous_file_to_database( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_imprt_previous_batch_file_to_database( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_show_download_properties_window( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_show_properties_history_window( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_connect_or_disconnect( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_save_option_as_defauilt( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_show_option_window( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_goto_online_help( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_check_for_a_new_version( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_about( wxCommandEvent& e )
{
	e.Skip(); 
}

void DownloadFrame::on_main_splitter_sash_position_changed( wxSplitterEvent& e )
{
	e.Skip(); 
}

