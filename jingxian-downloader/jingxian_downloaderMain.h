/***************************************************************
 * Name:      jinxian_downloaderMain.h
 * Purpose:   Defines Application Frame
 * Author:    runner.mei (runner.mei@gmail.com)
 * Created:   2008-10-31
 * Copyright: runner.mei ()
 * License:
 **************************************************************/

#ifndef JINXIAN_DOWNLOADERMAIN_H
#define JINXIAN_DOWNLOADERMAIN_H

#include <wx/wx.h>
#include "jingxian_downloaderApp.h"

class jinxian_downloaderFrame: public wxFrame
{
    public:
        jinxian_downloaderFrame(wxFrame *frame, const wxString& title);
        ~jinxian_downloaderFrame();
    private:
        enum
        {
            idMenuQuit = 1000,
            idMenuAbout
        };
        void OnClose(wxCloseEvent& event);
        void OnQuit(wxCommandEvent& event);
        void OnAbout(wxCommandEvent& event);
        DECLARE_EVENT_TABLE()
};


#endif // JINXIAN_DOWNLOADERMAIN_H
