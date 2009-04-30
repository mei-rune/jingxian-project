/***************************************************************
 * Name:      jinxian_downloaderApp.cpp
 * Purpose:   Code for Application Class
 * Author:    runner.mei (runner.mei@gmail.com)
 * Created:   2008-10-31
 * Copyright: runner.mei ()
 * License:
 **************************************************************/

#include "pre_config.h"
#include "wx/stdpaths.h"
#include "wx/filename.h"
#include "jxApp.h"
#include "DownloadFrame.h"
#include "VirtualFileSystem.h"
#include "icons/jingxian.xpm"

IMPLEMENT_APP(jxApp);

bool jxApp::OnInit()
{
#if defined(__WXMSW__)
	wxFileName name( wxStandardPathsBase::Get().GetExecutablePath());
    wxString binDirectory =  name.GetPath();
#else
    wxString binDirectory = wxStandardPaths::GetConfigDir() );
#endif

	VirtualFileSystem::Get().SetBaseDirectory( binDirectory );
    m_frame = new DownloadFrame(0L);
    m_frame->SetIcon(wxIcon(jingxian)); // To Set App Icon
    m_frame->Show();
    
    return true;
}
