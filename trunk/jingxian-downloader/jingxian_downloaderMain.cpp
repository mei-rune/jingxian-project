/***************************************************************
 * Name:      jinxian_downloaderMain.cpp
 * Purpose:   Code for Application Frame
 * Author:    runner.mei (runner.mei@gmail.com)
 * Created:   2008-10-31
 * Copyright: runner.mei ()
 * License:
 **************************************************************/

#include "pre_config.h"
#include "jingxian_downloaderMain.h"

//helper functions
enum wxbuildinfoformat {
    short_f, long_f };

wxString wxbuildinfo(wxbuildinfoformat format)
{
    wxString wxbuild(wxVERSION_STRING);

    if (format == long_f )
    {
#if defined(__WXMSW__)
        wxbuild << _T("-Windows");
#elif defined(__WXMAC__)
        wxbuild << _T("-Mac");
#elif defined(__UNIX__)
        wxbuild << _T("-Linux");
#endif

#if wxUSE_UNICODE
        wxbuild << _T("-Unicode build");
#else
        wxbuild << _T("-ANSI build");
#endif // wxUSE_UNICODE
    }

    return wxbuild;
}

BEGIN_EVENT_TABLE(jinxian_downloaderFrame, wxFrame)
    EVT_CLOSE(jinxian_downloaderFrame::OnClose)
    EVT_MENU(idMenuQuit, jinxian_downloaderFrame::OnQuit)
    EVT_MENU(idMenuAbout, jinxian_downloaderFrame::OnAbout)
END_EVENT_TABLE()

jinxian_downloaderFrame::jinxian_downloaderFrame(wxFrame *frame, const wxString& title)
    : wxFrame(frame, -1, title)
{
#if wxUSE_MENUS
    // create a menu bar
    wxMenuBar* mbar = new wxMenuBar();
    wxMenu* fileMenu = new wxMenu(_T(""));
    fileMenu->Append(idMenuQuit, _("&Quit\tAlt-F4"), _("Quit the application"));
    mbar->Append(fileMenu, _("&File"));

    wxMenu* helpMenu = new wxMenu(_T(""));
    helpMenu->Append(idMenuAbout, _("&About\tF1"), _("Show info about this application"));
    mbar->Append(helpMenu, _("&Help"));

    SetMenuBar(mbar);
#endif // wxUSE_MENUS

#if wxUSE_STATUSBAR
    // create a status bar with some information about the used wxWidgets version
    CreateStatusBar(2);
    SetStatusText(_("Hello Code::Blocks user!"),0);
    SetStatusText(wxbuildinfo(short_f), 1);
#endif // wxUSE_STATUSBAR

}


jinxian_downloaderFrame::~jinxian_downloaderFrame()
{
}

void jinxian_downloaderFrame::OnClose(wxCloseEvent &event)
{
    Destroy();
}

void jinxian_downloaderFrame::OnQuit(wxCommandEvent &event)
{
    Destroy();
}

void jinxian_downloaderFrame::OnAbout(wxCommandEvent &event)
{
    wxString msg = wxbuildinfo(long_f);
    wxMessageBox(msg, _("Welcome to..."));
}
