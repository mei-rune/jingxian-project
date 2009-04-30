///////////////////////////////////////////////////////////////////////////
// C++ code generated with wxFormBuilder (version Apr 16 2008)
// http://www.wxformbuilder.org/
//
// PLEASE DO "NOT" EDIT THIS FILE!
///////////////////////////////////////////////////////////////////////////

#include "pre_config.h"
#include "DownloadFrame.h"

///////////////////////////////////////////////////////////////////////////
DownloadFrame::DownloadFrame( wxWindow* parent, wxWindowID id, const wxString& title, const wxPoint& pos, const wxSize& size, long style ) : wxFrame( parent, id, title, pos, size, style )
{
	initialize();
	this->SetSizeHints( wxDefaultSize, wxDefaultSize );
	
	this->CreateStatusBar( );
	this->SetMenuBar( createMenuBar() );
	createToolBar();



	wxSplitterWindow* mainSplitter = new wxSplitterWindow( this, wxID_ANY, wxDefaultPosition, wxSize( -1,-1 ), 0 );
	//mainSplitter->Connect( wxEVT_IDLE, wxIdleEventHandler( DownloadFrame::on_splitterOnIdle ), NULL, this );

	
	m_download_group_wnd = new DownloadGroupWnd( mainSplitter );
	m_download_task_wnd = new DownloadTaskWnd( mainSplitter );

	mainSplitter->SplitVertically(m_download_group_wnd, m_download_task_wnd, 150 );

	wxBoxSizer* mainSizer = new wxBoxSizer( wxVERTICAL );
	mainSizer->Add( mainSplitter, 2, wxEXPAND, 0 );
	this->SetSizer( mainSizer );
	//this->SetAutoLayout( true );
	this->Layout();
	
	// Connect Events
	this->Connect( wxID_COMMAND_MENU_NEW_DOWNLOAD, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_new_download ) );
	this->Connect( wxID_COMMAND_MENU_ADD_BATCH_DOWNLOAD, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_add_batch_dwonload ) );
	this->Connect( wxID_COMMAND_MENU_LAUNCH_DOWNLOAD_FILE, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_launch_dwonload_file ) );
	this->Connect( wxID_COMMAND_MENU_BROWSE_FOLDER, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_browse_folder ) );
	this->Connect( wxID_COMMAND_MENU_CHECK_FOR_FILE_UPDATE, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_check_for_file_update ) );
	this->Connect( wxID_COMMAND_MENU_DOWNLOAD_AGAIN, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_download_again ) );
	this->Connect( wxID_COMMAND_MENU_START_DOWNLOAD, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_start_download ) );
	this->Connect( wxID_COMMAND_MENU_PAUSE_DOWNLOAD, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_pause_download ) );
	this->Connect( wxID_COMMAND_MENU_SCHEDULE_DOWNLOAD, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_schedule_download ) );
	this->Connect( wxID_COMMAND_MENU_START_DOWNLOAD_ALL, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_start_all_downloads ) );
	this->Connect( wxID_COMMAND_MENU_PAUSE_DOWNLOAD_ALL, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_pause_all_downloads ) );
	this->Connect( wxID_COMMAND_MENU_IMPORT_DOWNLOAD_LIST, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_import_download_list ) );
	this->Connect( wxID_COMMAND_MENU_IMPORT_BROKEN_DOWNLOADS, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_import_broken_downloads ) );
	this->Connect( wxID_COMMAND_MENU_IMPORT_LINKS_FROM_LOCAL_FILE, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_import_links_from_local_file ) );
	this->Connect( wxID_COMMAND_MENU_EXPORT_DOWNLOAD_LIST, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_export_download_list ) );
	this->Connect( wxID_COMMAND_MENU_EXPORT_DOWNLOAD_INFORMATION, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_export_download_information ) );
	this->Connect( wxID_COMMAND_MENU_EXIT, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_exit ) );
	this->Connect( wxID_COMMAND_MENU_SELECT_ALL, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_select_all_downloads ) );
	this->Connect( wxID_COMMAND_MENU_INVERT_SELECTION, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_invert_selection ) );
	this->Connect( wxID_COMMAND_MENU_FIND, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_find_download ) );
	this->Connect( wxID_COMMAND_MENU_FIND_NEXT, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_find_next_download ) );
	this->Connect( wxID_COMMAND_MENU_MOVE_UP, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_move_up ) );
	this->Connect( wxID_COMMAND_MENU_MOVE_DOWN, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_move_down ) );
	this->Connect( wxID_COMMAND_MENU_DELETE_DOWNLOAD, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_delete_download ) );
	this->Connect( wxID_COMMAND_MENU_MOVE_TO, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_move_to ) );
	this->Connect( wxID_COMMAND_MENU_COPY_URL_TO_CLIPBROAD, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_copy_url_to_clipbroad ) );
	this->Connect( wxID_COMMAND_MENU_VIEW_DETAIL, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_show_detail_panel ) );
	this->Connect( wxID_COMMAND_MENU_VIEW_DROP_ZONE, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_drop_zone_window ) );
	this->Connect( wxID_COMMAND_MENU_VIEW_CATEGORY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_show_class_panel ) );
	this->Connect( wxID_COMMAND_MENU_EXPORT_DOWNLOAD, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_export_download_file ) );
	this->Connect( wxID_COMMAND_MENU_RENAME_FILE, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_rename_file ) );
	this->Connect( wxID_COMMAND_MENU_COMMET_AS_FILENAME, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_rename_commet_as_filename ) );
	this->Connect( wxID_COMMAND_MENU_NEW_CATEGORY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_new_category ) );
	this->Connect( wxID_COMMAND_MENU_MOVE_CATEGORY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_move_category_to ) );
	this->Connect( wxID_COMMAND_MENU_DELETE_CATEGORY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_delete_category ) );
	this->Connect( wxID_COMMAND_MENU_CATEGORY_PROPERTIES, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_show_category_properties_window ) );
	this->Connect( wxID_COMMAND_MENU_NEW_DATABASE, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_new_database ) );
	this->Connect( wxID_COMMAND_MENU_OPEN_DATABASE, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_open_database ) );
	this->Connect( wxID_COMMAND_MENU_MERGE_DATABASE, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_merge_database ) );
	this->Connect( wxID_COMMAND_MENU_SAVE_DATABASE, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_save_database ) );
	this->Connect( wxID_COMMAND_MENU_BACKUP_TO_DATABASE, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_backup_to_database ) );
	this->Connect( wxID_COMMAND_MENU_IMPORT_DATABASE_FROM_PREVIOUS_FILE, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_imprt_previous_file_to_database ) );
	this->Connect( wxID_COMMAND_MENU_IMPORT_DATABASE_FROM_PREVIOUS_BATCH_FILE, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_imprt_previous_batch_file_to_database ) );
	this->Connect( wxID_COMMAND_MENU_VIEW_DOWNLOAD_PROPERTIES, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_show_download_properties_window ) );
	this->Connect( wxID_COMMAND_MENU_VIEW_PROPERTIES_HISTORY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_show_properties_history_window ) );
	this->Connect( wxID_COMMAND_MENU_CONNECT_OR_DISCONNECT, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_connect_or_disconnect ) );
	this->Connect( wxID_COMMAND_MENU_SAVE_AS_DEFAULT, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_save_option_as_defauilt ) );
	this->Connect( wxID_COMMAND_MENU_SHOW_DEFAULT_DOWNLOAD_PROPERTIES, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_show_download_properties_window ) );
	this->Connect( wxID_COMMAND_MENU_OPTIONS, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_show_option_window ) );
	this->Connect( wxID_COMMAND_MENU_HELP, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_goto_online_help ) );
	this->Connect( wxID_COMMAND_MENU_CHECK_FOR_A_NEW_VERSION, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_check_for_a_new_version ) );
	this->Connect( wxID_COMMAND_MENU_ABOUT, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_about ) );
	this->Connect( wxID_COMMAND_TOOL_NEW_DOWNLOAD, wxEVT_COMMAND_TOOL_CLICKED, wxCommandEventHandler( DownloadFrame::on_new_download ) );
	this->Connect( wxID_COMMAND_TOOL_START_DOWNLOAD, wxEVT_COMMAND_TOOL_CLICKED, wxCommandEventHandler( DownloadFrame::on_start_download ) );
	this->Connect( wxID_COMMAND_TOOL_PAUSE_DOWNLOAD, wxEVT_COMMAND_TOOL_CLICKED, wxCommandEventHandler( DownloadFrame::on_pause_download ) );
	this->Connect( wxID_COMMAND_TOOL_DELETE_DOWNLOAD, wxEVT_COMMAND_TOOL_CLICKED, wxCommandEventHandler( DownloadFrame::on_delete_download ) );
	this->Connect( wxID_COMMAND_TOOL_OPEN_DOWNLOAD, wxEVT_COMMAND_TOOL_CLICKED, wxCommandEventHandler( DownloadFrame::on_launch_dwonload_file ) );
	this->Connect( wxID_COMMAND_TOOL_BROWSE_FOLDER, wxEVT_COMMAND_TOOL_CLICKED, wxCommandEventHandler( DownloadFrame::on_browse_folder ) );
	this->Connect( wxID_COMMAND_TOOL_DOWNLOAD_PROPERTIES, wxEVT_COMMAND_TOOL_CLICKED, wxCommandEventHandler( DownloadFrame::on_show_download_properties_window ) );
	this->Connect( wxID_COMMAND_TOOL_MOVE_UP, wxEVT_COMMAND_TOOL_CLICKED, wxCommandEventHandler( DownloadFrame::on_move_up ) );
	this->Connect( wxID_COMMAND_TOOL_MOVE_DOWN, wxEVT_COMMAND_TOOL_CLICKED, wxCommandEventHandler( DownloadFrame::on_move_down ) );
	this->Connect( wxID_COMMAND_TOOL_DOWNLOAD_OPTION, wxEVT_COMMAND_TOOL_CLICKED, wxCommandEventHandler( DownloadFrame::on_show_option_window ) );
	this->Connect( wxID_COMMAND_TOOL_FIND_DOWNLOAD, wxEVT_COMMAND_TOOL_CLICKED, wxCommandEventHandler( DownloadFrame::on_find_download ) );
	this->Connect( wxID_COMMAND_TOOL_FIND_NEXT_DOWNLOAD, wxEVT_COMMAND_TOOL_CLICKED, wxCommandEventHandler( DownloadFrame::on_find_next_download ) );
	//mainSplitter->Connect( wxEVT_COMMAND_SPLITTER_SASH_POS_CHANGED, wxSplitterEventHandler( DownloadFrame::on_main_splitter_sash_position_changed ), NULL, this );	
}

DownloadFrame::~DownloadFrame()
{
	// Disconnect Events
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_new_download ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_add_batch_dwonload ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_launch_dwonload_file ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_browse_folder ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_check_for_file_update ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_download_again ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_start_download ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_pause_download ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_schedule_download ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_start_all_downloads ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_pause_all_downloads ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_import_download_list ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_import_broken_downloads ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_import_links_from_local_file ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_export_download_list ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_export_download_information ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_exit ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_select_all_downloads ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_invert_selection ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_find_download ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_find_next_download ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_move_up ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_move_down ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_delete_download ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_move_to ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_copy_url_to_clipbroad ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_show_detail_panel ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_drop_zone_window ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_show_class_panel ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_export_download_file ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_rename_file ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_rename_commet_as_filename ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_new_category ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_move_category_to ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_delete_category ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_show_category_properties_window ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_new_database ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_open_database ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_merge_database ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_save_database ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_backup_to_database ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_imprt_previous_file_to_database ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_imprt_previous_batch_file_to_database ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_show_download_properties_window ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_show_properties_history_window ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_connect_or_disconnect ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_save_option_as_defauilt ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_show_download_properties_window ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_show_option_window ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_goto_online_help ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_check_for_a_new_version ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler( DownloadFrame::on_about ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_TOOL_CLICKED, wxCommandEventHandler( DownloadFrame::on_new_download ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_TOOL_CLICKED, wxCommandEventHandler( DownloadFrame::on_start_download ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_TOOL_CLICKED, wxCommandEventHandler( DownloadFrame::on_pause_download ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_TOOL_CLICKED, wxCommandEventHandler( DownloadFrame::on_delete_download ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_TOOL_CLICKED, wxCommandEventHandler( DownloadFrame::on_launch_dwonload_file ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_TOOL_CLICKED, wxCommandEventHandler( DownloadFrame::on_browse_folder ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_TOOL_CLICKED, wxCommandEventHandler( DownloadFrame::on_show_download_properties_window ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_TOOL_CLICKED, wxCommandEventHandler( DownloadFrame::on_move_up ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_TOOL_CLICKED, wxCommandEventHandler( DownloadFrame::on_move_down ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_TOOL_CLICKED, wxCommandEventHandler( DownloadFrame::on_show_option_window ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_TOOL_CLICKED, wxCommandEventHandler( DownloadFrame::on_find_download ) );
	this->Disconnect( wxID_ANY, wxEVT_COMMAND_TOOL_CLICKED, wxCommandEventHandler( DownloadFrame::on_find_next_download ) );
	//mainSplitter->Disconnect( wxEVT_COMMAND_SPLITTER_SASH_POS_CHANGED, wxSplitterEventHandler( DownloadFrame::on_main_splitter_sash_position_changed ), NULL, this );
}


wxToolBar* DownloadFrame::createToolBar()
{
	wxToolBar* toolBar = this->CreateToolBar( wxTB_DOCKABLE|wxTB_FLAT|wxTB_HORIZONTAL, wxID_ANY );
	toolBar->SetToolBitmapSize( wxSize( 24,24 ) );
	toolBar->AddTool( wxID_COMMAND_TOOL_NEW_DOWNLOAD, wxT("New"), m_bitmap_new_download[1], wxNullBitmap );
	toolBar->AddTool( wxID_COMMAND_TOOL_START_DOWNLOAD, wxT("Start"), m_bitmap_start_download[1], wxNullBitmap );
	toolBar->AddTool( wxID_COMMAND_TOOL_PAUSE_DOWNLOAD, wxT("Pause"), m_bitmap_pause_download[1], wxNullBitmap );
	toolBar->AddTool( wxID_COMMAND_TOOL_DELETE_DOWNLOAD, wxT("Delete"), m_bitmap_delete_download[1], wxNullBitmap );
	toolBar->AddTool( wxID_COMMAND_TOOL_OPEN_DOWNLOAD, wxT("Open"), m_bitmap_open_download_file[1], wxNullBitmap );
	toolBar->AddTool( wxID_COMMAND_TOOL_BROWSE_FOLDER, wxT("Directory"), m_bitmap_browse_folder[1], wxNullBitmap );
	toolBar->AddTool( wxID_COMMAND_TOOL_DOWNLOAD_PROPERTIES, wxT("Properties"), m_bitmap_download_properties[1], wxNullBitmap );
	toolBar->AddTool( wxID_COMMAND_TOOL_MOVE_UP, wxT("Up"), m_bitmap_move_up[1], wxNullBitmap );
	toolBar->AddTool( wxID_COMMAND_TOOL_MOVE_DOWN, wxT("Down"), m_bitmap_move_down[1], wxNullBitmap );
	toolBar->AddTool( wxID_COMMAND_TOOL_DOWNLOAD_OPTION, wxT("Option"), m_bitmap_options[1], wxNullBitmap );
	toolBar->AddTool( wxID_COMMAND_TOOL_FIND_DOWNLOAD, wxT("Find"), m_bitmap_find[1], wxNullBitmap );
	toolBar->AddTool( wxID_COMMAND_TOOL_FIND_NEXT_DOWNLOAD, wxT("Next"), m_bitmap_find_next[1], wxNullBitmap );
	toolBar->Realize();

	return toolBar;
}


wxMenuBar* DownloadFrame::createMenuBar()
{
	wxMenuBar* menuBar  = new wxMenuBar( 0 );
	wxMenu* fileMenu = new wxMenu();
	
	fileMenu->Append( new wxMenuItem( fileMenu, wxID_COMMAND_MENU_NEW_DOWNLOAD, wxString( wxT("&New Download ...") ) + wxT('\t') + wxT("F4")) );
	fileMenu->Append( new wxMenuItem( fileMenu, wxID_COMMAND_MENU_ADD_BATCH_DOWNLOAD, wxString( wxT("Add &batch download ...") ) ) );
	fileMenu->AppendSeparator();

	fileMenu->Append( new wxMenuItem( fileMenu, wxID_COMMAND_MENU_LAUNCH_DOWNLOAD_FILE, wxString( wxT("Launch download &file") ) + wxT('\t') + wxT("Enter")) );
	fileMenu->Append( new wxMenuItem( fileMenu, wxID_COMMAND_MENU_BROWSE_FOLDER, wxString( wxT("&Browse folder") ) + wxT('\t') + wxT("CTRL+Enter")) );
	fileMenu->Append( new wxMenuItem( fileMenu, wxID_COMMAND_MENU_CHECK_FOR_FILE_UPDATE, wxString( wxT("&Check for file update") ) ) );
	fileMenu->Append( new wxMenuItem( fileMenu, wxID_COMMAND_MENU_DOWNLOAD_AGAIN, wxString( wxT("&Download again") ) ) );
	fileMenu->AppendSeparator();
	
	fileMenu->Append( new wxMenuItem( fileMenu, wxID_COMMAND_MENU_START_DOWNLOAD, wxString( wxT("&Start") ) + wxT('\t') + wxT("F5")) );
	fileMenu->Append( new wxMenuItem( fileMenu, wxID_COMMAND_MENU_PAUSE_DOWNLOAD, wxString( wxT("&Pause") ) + wxT('\t') + wxT("F6")) );
	fileMenu->Append( new wxMenuItem( fileMenu, wxID_COMMAND_MENU_SCHEDULE_DOWNLOAD, wxString( wxT("Schedule") ) + wxT('\t') + wxT("ALT+S")) );
	fileMenu->Append( new wxMenuItem( fileMenu, wxID_COMMAND_MENU_START_DOWNLOAD_ALL, wxString( wxT("Start All") ) + wxT('\t') + wxT("F8")) );
	fileMenu->Append( new wxMenuItem( fileMenu, wxID_COMMAND_MENU_PAUSE_DOWNLOAD_ALL, wxString( wxT("Pause All") ) + wxT('\t') + wxT("F9")) );
	fileMenu->AppendSeparator();

	wxMenu* import_menu = new wxMenu();
	import_menu->Append( new wxMenuItem( import_menu, wxID_COMMAND_MENU_IMPORT_DOWNLOAD_LIST, wxString( wxT("Import list...") ) ) );
	import_menu->Append( new wxMenuItem( import_menu, wxID_COMMAND_MENU_IMPORT_BROKEN_DOWNLOADS, wxString( wxT("Import Broken Downloads...") ) ) );
	import_menu->Append( new wxMenuItem( import_menu, wxID_COMMAND_MENU_IMPORT_LINKS_FROM_LOCAL_FILE, wxString( wxT("Import links from local file...") ) ) );
	fileMenu->Append( -1, wxT("Import"), import_menu );
	
	wxMenu* export_menu = new wxMenu();
	export_menu->Append( new wxMenuItem( export_menu, wxID_COMMAND_MENU_EXPORT_DOWNLOAD_LIST, wxString( wxT("Export list...") ) ) );
	export_menu->Append( new wxMenuItem( export_menu, wxID_COMMAND_MENU_EXPORT_DOWNLOAD_INFORMATION, wxString( wxT("Export Information...") ) ) );
	fileMenu->Append( -1, wxT("Export"), export_menu );
	
	fileMenu->AppendSeparator();
	fileMenu->Append( new wxMenuItem( fileMenu, wxID_COMMAND_MENU_EXIT, wxString( wxT("E&xit") ) + wxT('\t') + wxT("x")) );
	menuBar->Append( fileMenu, wxT("&File") );
	
	wxMenu* editMenu = new wxMenu();
	editMenu->Append( new wxMenuItem( editMenu, wxID_COMMAND_MENU_SELECT_ALL, wxString( wxT("Select &All") ) + wxT('\t') + wxT("CTRL+A")) );
	editMenu->Append( new wxMenuItem( editMenu, wxID_COMMAND_MENU_INVERT_SELECTION, wxString( wxT("&Invert selection") ) ) );
	editMenu->AppendSeparator();
	
	editMenu->Append( new wxMenuItem( editMenu, wxID_COMMAND_MENU_FIND, wxString( wxT("&Find...") ) + wxT('\t') + wxT("CTRL+F")) );
	editMenu->Append( new wxMenuItem( editMenu, wxID_COMMAND_MENU_FIND_NEXT, wxString( wxT("Find &Next") ) + wxT('\t') + wxT("F3")) );
	editMenu->AppendSeparator();
	
	editMenu->Append( new wxMenuItem( editMenu, wxID_COMMAND_MENU_MOVE_UP, wxString( wxT("Move &Up") ) + wxT('\t') + wxT("ALT+U")) );
	editMenu->Append( new wxMenuItem( editMenu, wxID_COMMAND_MENU_MOVE_DOWN, wxString( wxT("Move &Down") ) + wxT('\t') + wxT("ALT+Down")) );
	editMenu->Append( new wxMenuItem( editMenu, wxID_COMMAND_MENU_DELETE_DOWNLOAD, wxString( wxT("&Delete") ) + wxT('\t') + wxT("ALT+Del")) );
	editMenu->Append( new wxMenuItem( editMenu, wxID_COMMAND_MENU_MOVE_TO, wxString( wxT("&Move To...") ) ) );
	editMenu->AppendSeparator();
	
	editMenu->Append( new wxMenuItem( editMenu, wxID_COMMAND_MENU_COPY_URL_TO_CLIPBROAD, wxString( wxT("&Copy URL to clipbroad") ) ) );
	
	menuBar->Append( editMenu, wxT("&Edit") );
	
	wxMenu* viewMenu = new wxMenu();
	viewMenu->Append( new wxMenuItem( viewMenu, wxID_COMMAND_MENU_VIEW_TOOLBAR, wxString( wxT("&Toolbar") ) ) );
	viewMenu->Append( new wxMenuItem( viewMenu, wxID_COMMAND_MENU_VIEW_STATUSBAR, wxString( wxT("&StatusBar") ) ) );
	viewMenu->Append( new wxMenuItem( viewMenu, wxID_COMMAND_MENU_VIEW_DETAIL, wxString( wxT("&Detail") ) ) );
	viewMenu->Append( new wxMenuItem( viewMenu, wxID_COMMAND_MENU_VIEW_DROP_ZONE, wxString( wxT("Drop &Zone") ) ) );
	viewMenu->Append( new wxMenuItem( viewMenu, wxID_COMMAND_MENU_VIEW_CATEGORY, wxString( wxT("&Category") ) ) );
	
	viewMenu->AppendSeparator();
	
	wxMenu* language_menu = new wxMenu();
	viewMenu->Append( -1, wxT("&Language"), language_menu );
	
	menuBar->Append( viewMenu, wxT("&View") );
	
	wxMenu* manageMenu = new wxMenu();
	manageMenu->Append( new wxMenuItem( manageMenu, wxID_COMMAND_MENU_EXPORT_DOWNLOAD, wxString( wxT("Export...") ) ) );
	
	wxMenu* rename_menu = new wxMenu();
	rename_menu->Append( new wxMenuItem( rename_menu, wxID_COMMAND_MENU_RENAME_FILE, wxString( wxT("Rename...") ) ) );
	rename_menu->Append( new wxMenuItem( rename_menu, wxID_COMMAND_MENU_COMMET_AS_FILENAME, wxString( wxT("Commet as Filename") ) ) );
	
	manageMenu->Append( -1, wxT("&Rename..."), rename_menu );
	
	manageMenu->AppendSeparator();
	
	manageMenu->Append( new wxMenuItem( manageMenu, wxID_COMMAND_MENU_NEW_CATEGORY, wxString( wxT("&new category...") ) ) );
	manageMenu->Append( new wxMenuItem( manageMenu, wxID_COMMAND_MENU_MOVE_CATEGORY, wxString( wxT("&Move category to...") ) ) );
	manageMenu->Append( new wxMenuItem( manageMenu, wxID_COMMAND_MENU_DELETE_CATEGORY, wxString( wxT("&Delete category") ) ) );
	manageMenu->Append( new wxMenuItem( manageMenu, wxID_COMMAND_MENU_CATEGORY_PROPERTIES, wxString( wxT("&Category properties...") ) ) );
	
	manageMenu->AppendSeparator();
	
	wxMenu* database_menu = new wxMenu();
	database_menu->Append( new wxMenuItem( database_menu, wxID_COMMAND_MENU_NEW_DATABASE, wxString( wxT("&New database...") ) ) );
	database_menu->Append( new wxMenuItem( database_menu, wxID_COMMAND_MENU_OPEN_DATABASE, wxString( wxT("&Open database") ) ) );
	database_menu->Append( new wxMenuItem( database_menu, wxID_COMMAND_MENU_MERGE_DATABASE, wxString( wxT("&Merge database...") ) ) );
	database_menu->Append( new wxMenuItem( database_menu, wxID_COMMAND_MENU_SAVE_DATABASE, wxString( wxT("&Save database...") ) ) );
	database_menu->Append( new wxMenuItem( database_menu, wxID_COMMAND_MENU_BACKUP_TO_DATABASE, wxString( wxT("&Backup to...") ) ) );
	database_menu->Append( new wxMenuItem( database_menu, wxID_COMMAND_MENU_IMPORT_DATABASE_FROM_PREVIOUS_FILE, wxString( wxT("Import &Previous File...") ) ) );
	database_menu->Append( new wxMenuItem( database_menu, wxID_COMMAND_MENU_IMPORT_DATABASE_FROM_PREVIOUS_BATCH_FILE, wxString( wxT("Import Previous &Batch File...") ) ) );
	
	manageMenu->Append( -1, wxT("&Download database"), database_menu );
	
	manageMenu->AppendSeparator();
	
	manageMenu->Append( new wxMenuItem( manageMenu, wxID_COMMAND_MENU_VIEW_DOWNLOAD_PROPERTIES, wxString( wxT("View download properties...") ) + wxT('\t') + wxT("ALT+Enter")) );
	manageMenu->Append( new wxMenuItem( manageMenu, wxID_COMMAND_MENU_VIEW_PROPERTIES_HISTORY, wxString( wxT("&View Properties History...") ) + wxT('\t') + wxT("CTRL+R")) );
	
	menuBar->Append( manageMenu, wxT("&Manage") );
	
	wxMenu* toolMenu = new wxMenu();
	toolMenu->Append( new wxMenuItem( toolMenu, wxID_COMMAND_MENU_CONNECT_OR_DISCONNECT, wxString( wxT("&Connect/Disconnect") ) ) );
	
	toolMenu->AppendSeparator();

	toolMenu->Append( new wxMenuItem( toolMenu, wxID_COMMAND_MENU_SAVE_AS_DEFAULT, wxString( wxT("&Save as default") ) ) );
	toolMenu->Append( new wxMenuItem( toolMenu, wxID_COMMAND_MENU_SHOW_DEFAULT_DOWNLOAD_PROPERTIES, wxString( wxT("&Default download properties...") ) ) );
	toolMenu->Append( new wxMenuItem( toolMenu, wxID_COMMAND_MENU_OPTIONS, wxString( wxT("&Option...") ) ) );
	
	menuBar->Append( toolMenu, wxT("&Tools") );
	
	wxMenu* helpMenu = new wxMenu();
	helpMenu->Append( new wxMenuItem( helpMenu, wxID_COMMAND_MENU_HELP, wxString( wxT("&Online help") ) ) );
	helpMenu->Append( new wxMenuItem( helpMenu, wxID_COMMAND_MENU_CHECK_FOR_A_NEW_VERSION, wxString( wxT("&Check for a new version...") ) ) );
	helpMenu->Append( new wxMenuItem( helpMenu, wxID_COMMAND_MENU_ABOUT, wxString( wxT("&About...") ) ) );

	menuBar->Append( helpMenu, wxT("&Help") );

	return menuBar;
}




optionDialog::optionDialog( wxWindow* parent, wxWindowID id, const wxString& title, const wxPoint& pos, const wxSize& size, long style ) : wxDialog( parent, id, title, pos, size, style )
{
	this->SetSizeHints( wxDefaultSize, wxDefaultSize );
	
	wxBoxSizer* m_mainSizer;
	m_mainSizer = new wxBoxSizer( wxHORIZONTAL );
	
	m_option_tree = new wxTreeCtrl( this, wxID_ANY, wxDefaultPosition, wxSize( 200,-1 ), wxTR_DEFAULT_STYLE );
	m_option_tree->SetMinSize( wxSize( 200,-1 ) );
	m_option_tree->SetMaxSize( wxSize( 200,-1 ) );
	
	m_mainSizer->Add( m_option_tree, 0, wxALL|wxEXPAND, 5 );
	
	this->SetSizer( m_mainSizer );
	this->Layout();
}

optionDialog::~optionDialog()
{
}
