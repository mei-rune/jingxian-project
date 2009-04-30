///////////////////////////////////////////////////////////////////////////
// C++ code generated with wxFormBuilder (version Apr 16 2008)
// http://www.wxformbuilder.org/
//
// PLEASE DO "NOT" EDIT THIS FILE!
///////////////////////////////////////////////////////////////////////////

#include "pre_config.h"
#include "NewDownload.h"

///////////////////////////////////////////////////////////////////////////

NewDownload::NewDownload( wxWindow* parent, wxWindowID id, const wxString& title, const wxPoint& pos, const wxSize& size, long style ) : wxDialog( parent, id, title, pos, size, style )
{
	this->SetSizeHints( wxDefaultSize, wxDefaultSize );
	
	wxBoxSizer* topSizer;
	topSizer = new wxBoxSizer( wxVERTICAL );
	
	m_option_auinotebook = new wxAuiNotebook( this, wxID_ANY, wxPoint( -1,-1 ), wxDefaultSize, wxAUI_NB_TAB_EXTERNAL_MOVE );
	m_general_option_panel = new wxPanel( m_option_auinotebook, wxID_ANY, wxDefaultPosition, wxDefaultSize, wxTAB_TRAVERSAL );
	wxBoxSizer* bSizer37;
	bSizer37 = new wxBoxSizer( wxVERTICAL );
	
	wxFlexGridSizer* urlSizer;
	urlSizer = new wxFlexGridSizer( 2, 2, 0, 0 );
	urlSizer->SetFlexibleDirection( wxVERTICAL );
	urlSizer->SetNonFlexibleGrowMode( wxFLEX_GROWMODE_ALL );
	
	m_staticText5 = new wxStaticText( m_general_option_panel, wxID_ANY, wxT("MyLabel"), wxDefaultPosition, wxDefaultSize, 0 );
	m_staticText5->Wrap( -1 );
	urlSizer->Add( m_staticText5, 0, wxALL, 5 );
	
	m_textCtrl4 = new wxTextCtrl( m_general_option_panel, wxID_ANY, wxEmptyString, wxDefaultPosition, wxDefaultSize, 0 );
	m_textCtrl4->SetMaxLength( 2048 ); 
	urlSizer->Add( m_textCtrl4, 1, wxALL|wxEXPAND, 5 );
	
	m_staticText6 = new wxStaticText( m_general_option_panel, wxID_ANY, wxT("MyLabel"), wxDefaultPosition, wxDefaultSize, 0 );
	m_staticText6->Wrap( -1 );
	urlSizer->Add( m_staticText6, 0, wxALL, 5 );
	
	m_textCtrl5 = new wxTextCtrl( m_general_option_panel, wxID_ANY, wxEmptyString, wxDefaultPosition, wxDefaultSize, 0 );
	urlSizer->Add( m_textCtrl5, 1, wxALL|wxEXPAND, 5 );
	
	bSizer37->Add( urlSizer, 1, wxEXPAND, 5 );
	
	m_staticline5 = new wxStaticLine( m_general_option_panel, wxID_ANY, wxDefaultPosition, wxDefaultSize, wxLI_HORIZONTAL );
	bSizer37->Add( m_staticline5, 0, wxEXPAND | wxALL, 5 );
	
	wxFlexGridSizer* fileSizer;
	fileSizer = new wxFlexGridSizer( 3, 3, 0, 0 );
	fileSizer->SetFlexibleDirection( wxBOTH );
	fileSizer->SetNonFlexibleGrowMode( wxFLEX_GROWMODE_SPECIFIED );
	
	m_staticText7 = new wxStaticText( m_general_option_panel, wxID_ANY, wxT("MyLabel"), wxDefaultPosition, wxDefaultSize, 0 );
	m_staticText7->Wrap( -1 );
	fileSizer->Add( m_staticText7, 0, wxALL, 5 );
	
	m_textCtrl6 = new wxTextCtrl( m_general_option_panel, wxID_ANY, wxEmptyString, wxDefaultPosition, wxDefaultSize, 0 );
	fileSizer->Add( m_textCtrl6, 0, wxALL, 5 );
	
	m_staticText11 = new wxStaticText( m_general_option_panel, wxID_ANY, wxT("MyLabel"), wxDefaultPosition, wxDefaultSize, 0 );
	m_staticText11->Wrap( -1 );
	fileSizer->Add( m_staticText11, 0, wxALL, 5 );
	
	m_staticText9 = new wxStaticText( m_general_option_panel, wxID_ANY, wxT("MyLabel"), wxDefaultPosition, wxDefaultSize, 0 );
	m_staticText9->Wrap( -1 );
	fileSizer->Add( m_staticText9, 0, wxALL, 5 );
	
	m_textCtrl8 = new wxTextCtrl( m_general_option_panel, wxID_ANY, wxEmptyString, wxDefaultPosition, wxDefaultSize, 0 );
	fileSizer->Add( m_textCtrl8, 0, wxALL, 5 );
	
	m_button7 = new wxButton( m_general_option_panel, wxID_ANY, wxT("MyButton"), wxDefaultPosition, wxDefaultSize, 0 );
	fileSizer->Add( m_button7, 0, wxALL, 5 );
	
	m_staticText10 = new wxStaticText( m_general_option_panel, wxID_ANY, wxT("MyLabel"), wxDefaultPosition, wxDefaultSize, 0 );
	m_staticText10->Wrap( -1 );
	fileSizer->Add( m_staticText10, 0, wxALL, 5 );
	
	m_textCtrl9 = new wxTextCtrl( m_general_option_panel, wxID_ANY, wxEmptyString, wxDefaultPosition, wxDefaultSize, 0 );
	fileSizer->Add( m_textCtrl9, 0, wxALL, 5 );
	
	m_button8 = new wxButton( m_general_option_panel, wxID_ANY, wxT("MyButton"), wxDefaultPosition, wxDefaultSize, 0 );
	fileSizer->Add( m_button8, 0, wxALL, 5 );
	
	bSizer37->Add( fileSizer, 1, wxEXPAND, 5 );
	
	m_general_option_panel->SetSizer( bSizer37 );
	m_general_option_panel->Layout();
	bSizer37->Fit( m_general_option_panel );
	m_option_auinotebook->AddPage( m_general_option_panel, wxT("General"), false, wxNullBitmap );
	m_advencd_option = new wxPanel( m_option_auinotebook, wxID_ANY, wxDefaultPosition, wxDefaultSize, wxTAB_TRAVERSAL );
	wxBoxSizer* bSizer14;
	bSizer14 = new wxBoxSizer( wxHORIZONTAL );
	
	wxGridBagSizer* gbSizer2;
	gbSizer2 = new wxGridBagSizer( 0, 0 );
	gbSizer2->SetFlexibleDirection( wxBOTH );
	gbSizer2->SetNonFlexibleGrowMode( wxFLEX_GROWMODE_SPECIFIED );
	
	m_login_checkBox = new wxCheckBox( m_advencd_option, wxID_ANY, wxT("Check Me!"), wxDefaultPosition, wxDefaultSize, 0 );
	
	gbSizer2->Add( m_login_checkBox, wxGBPosition( 0, 0 ), wxGBSpan( 1, 2 ), wxALL|wxEXPAND, 5 );
	
	m_user_label = new wxStaticText( m_advencd_option, wxID_ANY, wxT("Username:"), wxDefaultPosition, wxDefaultSize, 0 );
	m_user_label->Wrap( -1 );
	gbSizer2->Add( m_user_label, wxGBPosition( 1, 0 ), wxGBSpan( 1, 1 ), wxALL, 5 );
	
	m_user_name_box = new wxTextCtrl( m_advencd_option, wxID_ANY, wxEmptyString, wxDefaultPosition, wxDefaultSize, 0 );
	gbSizer2->Add( m_user_name_box, wxGBPosition( 1, 1 ), wxGBSpan( 1, 1 ), wxALL|wxEXPAND, 5 );
	
	m_password_label = new wxStaticText( m_advencd_option, wxID_ANY, wxT("Password:"), wxDefaultPosition, wxDefaultSize, 0 );
	m_password_label->Wrap( -1 );
	gbSizer2->Add( m_password_label, wxGBPosition( 2, 0 ), wxGBSpan( 1, 1 ), wxALL, 5 );
	
	m_password_box = new wxTextCtrl( m_advencd_option, wxID_ANY, wxEmptyString, wxDefaultPosition, wxDefaultSize, 0 );
	gbSizer2->Add( m_password_box, wxGBPosition( 2, 1 ), wxGBSpan( 1, 1 ), wxALL|wxEXPAND, 5 );
	
	m_textCtrl12 = new wxTextCtrl( m_advencd_option, wxID_ANY, wxEmptyString, wxDefaultPosition, wxDefaultSize, 0 );
	m_textCtrl12->SetMaxLength( 256 ); 
	gbSizer2->Add( m_textCtrl12, wxGBPosition( 4, 0 ), wxGBSpan( 5, 2 ), wxALL|wxEXPAND, 5 );
	
	m_staticText13 = new wxStaticText( m_advencd_option, wxID_ANY, wxT("MyLabel"), wxDefaultPosition, wxDefaultSize, 0 );
	m_staticText13->Wrap( -1 );
	gbSizer2->Add( m_staticText13, wxGBPosition( 3, 0 ), wxGBSpan( 1, 2 ), wxALL|wxEXPAND, 5 );
	
	bSizer14->Add( gbSizer2, 1, wxEXPAND, 5 );
	
	wxGridBagSizer* gbSizer4;
	gbSizer4 = new wxGridBagSizer( 0, 0 );
	gbSizer4->SetFlexibleDirection( wxBOTH );
	gbSizer4->SetNonFlexibleGrowMode( wxFLEX_GROWMODE_SPECIFIED );
	
	bSizer14->Add( gbSizer4, 1, wxEXPAND, 5 );
	
	m_advencd_option->SetSizer( bSizer14 );
	m_advencd_option->Layout();
	bSizer14->Fit( m_advencd_option );
	m_option_auinotebook->AddPage( m_advencd_option, wxT("Advanced"), true, wxNullBitmap );
	m_connect_option_panel = new wxPanel( m_option_auinotebook, wxID_ANY, wxDefaultPosition, wxDefaultSize, wxTAB_TRAVERSAL );
	m_option_auinotebook->AddPage( m_connect_option_panel, wxT("Connect"), false, wxNullBitmap );
	
	topSizer->Add( m_option_auinotebook, 1, wxEXPAND | wxALL, 5 );
	
	m_staticline17 = new wxStaticLine( this, wxID_ANY, wxDefaultPosition, wxDefaultSize, wxLI_HORIZONTAL );
	topSizer->Add( m_staticline17, 0, wxEXPAND | wxALL, 5 );
	
	wxBoxSizer* buttomSizer = new wxBoxSizer( wxHORIZONTAL );
	
	buttomSizer->Add( new wxButton( this, wxID_ANY, wxT("MyButton"), wxDefaultPosition, wxDefaultSize, 0 ), 0, wxALL, 5 );
	
	buttomSizer->Add( new wxButton( this, wxID_ANY, wxT("MyButton"), wxDefaultPosition, wxDefaultSize, 0 ), 0, wxALL, 5 );
	
	topSizer->Add( buttomSizer);
	
	this->SetSizer( topSizer );
	this->Layout();
}

NewDownload::~NewDownload()
{
}
