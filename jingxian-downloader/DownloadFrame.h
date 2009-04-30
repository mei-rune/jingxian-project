///////////////////////////////////////////////////////////////////////////
// C++ code generated with wxFormBuilder (version Apr 16 2008)
// http://www.wxformbuilder.org/
//
// PLEASE DO "NOT" EDIT THIS FILE!
///////////////////////////////////////////////////////////////////////////

#ifndef MAINFRAME_H
#define MAINFRAME_H

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
#include "jx.h"
#include "DownloadTaskWnd.h"
#include "DownloadGroupWnd.h"

///////////////////////////////////////////////////////////////////////////////
/// Class DownloadFrame
///////////////////////////////////////////////////////////////////////////////
class DownloadFrame : public wxFrame 
{
	private:

		DownloadGroupWnd* m_download_group_wnd;
		DownloadTaskWnd* m_download_task_wnd;

		wxBitmap m_bitmap_browse_folder[2];
		wxBitmap m_bitmap_delete_download[2];
		wxBitmap m_bitmap_download_properties[2];
		wxBitmap m_bitmap_find[2];
		wxBitmap m_bitmap_find_next[2];
		wxBitmap m_bitmap_move_down[2];
		wxBitmap m_bitmap_move_up[2];
		wxBitmap m_bitmap_new_download[2];
		wxBitmap m_bitmap_open_download_file[2];
		wxBitmap m_bitmap_options[2];
		wxBitmap m_bitmap_pause_download[2];
		wxBitmap m_bitmap_start_download[2];

		void initBitmap(const wxBitmap& xpm, int index );
		void initialize();

		void initializeDownloadList();
		void initializeCategoryTree();

		wxToolBar* createToolBar();
		wxMenuBar* createMenuBar();
        //wxPanel* createDownloadPanel( wxWindow* mainSplitter );

	protected:
		
		// event handlers, overide them in your derived class
		void on_new_download( wxCommandEvent& e );
		void on_add_batch_dwonload( wxCommandEvent& e );
		void on_launch_dwonload_file( wxCommandEvent& e );
		void on_browse_folder( wxCommandEvent& e );
		void on_check_for_file_update( wxCommandEvent& e );
		void on_download_again( wxCommandEvent& e );
		void on_start_download( wxCommandEvent& e );
		void on_pause_download( wxCommandEvent& e );
		void on_schedule_download( wxCommandEvent& e );
		void on_start_all_downloads( wxCommandEvent& e );
		void on_pause_all_downloads( wxCommandEvent& e );
		void on_import_download_list( wxCommandEvent& e );
		void on_import_broken_downloads( wxCommandEvent& e );
		void on_import_links_from_local_file( wxCommandEvent& e );
		void on_export_download_list( wxCommandEvent& e );
		void on_export_download_information( wxCommandEvent& e );
		void on_exit( wxCommandEvent& e );
		void on_select_all_downloads( wxCommandEvent& e );
		void on_invert_selection( wxCommandEvent& e );
		void on_find_download( wxCommandEvent& e );
		void on_find_next_download( wxCommandEvent& e );
		void on_move_up( wxCommandEvent& e );
		void on_move_down( wxCommandEvent& e );
		void on_delete_download( wxCommandEvent& e );
		void on_move_to( wxCommandEvent& e );
		void on_copy_url_to_clipbroad( wxCommandEvent& e );
		void on_show_detail_panel( wxCommandEvent& e );
		void on_drop_zone_window( wxCommandEvent& e );
		void on_show_class_panel( wxCommandEvent& e );
		void on_export_download_file( wxCommandEvent& e );
		void on_rename_file( wxCommandEvent& e );
		void on_rename_commet_as_filename( wxCommandEvent& e );
		void on_new_category( wxCommandEvent& e );
		void on_move_category_to( wxCommandEvent& e );
		void on_delete_category( wxCommandEvent& e );
		void on_show_category_properties_window( wxCommandEvent& e );
		void on_new_database( wxCommandEvent& e );
		void on_open_database( wxCommandEvent& e );
		void on_merge_database( wxCommandEvent& e );
		void on_save_database( wxCommandEvent& e );
		void on_backup_to_database( wxCommandEvent& e );
		void on_imprt_previous_file_to_database( wxCommandEvent& e );
		void on_imprt_previous_batch_file_to_database( wxCommandEvent& e );
		void on_show_download_properties_window( wxCommandEvent& e );
		void on_show_properties_history_window( wxCommandEvent& e );
		void on_connect_or_disconnect( wxCommandEvent& e );
		void on_save_option_as_defauilt( wxCommandEvent& e );
		void on_show_option_window( wxCommandEvent& e );
		void on_goto_online_help( wxCommandEvent& e );
		void on_check_for_a_new_version( wxCommandEvent& e );
		void on_about( wxCommandEvent& e );
		void on_main_splitter_sash_position_changed( wxSplitterEvent& e );
		void on_category_right_click( wxTreeEvent& e );
		void on_category_select_changed( wxTreeEvent& e );

		//wxPanel* createTreePanel( wxWindow* mainSplitter );
	public:
		DownloadFrame( wxWindow* parent, wxWindowID id = wxID_ANY, const wxString& title = wxT("jinxian downloader"), const wxPoint& pos = wxDefaultPosition, const wxSize& size = wxSize( 530,495 ), long style = wxDEFAULT_FRAME_STYLE|wxTAB_TRAVERSAL );
		~DownloadFrame();
};

///////////////////////////////////////////////////////////////////////////////
/// Class optionDialog
///////////////////////////////////////////////////////////////////////////////
class optionDialog : public wxDialog 
{
	private:
	
	protected:
		wxTreeCtrl* m_option_tree;
	
	public:
		optionDialog( wxWindow* parent, wxWindowID id = wxID_ANY, const wxString& title = wxT("Options..."), const wxPoint& pos = wxDefaultPosition, const wxSize& size = wxSize( 689,551 ), long style = wxDEFAULT_DIALOG_STYLE );
		~optionDialog();
	
};

#endif //MAINFRAME_H
