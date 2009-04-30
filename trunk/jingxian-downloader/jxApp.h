/***************************************************************
 * Name:      jinxian_downloaderApp.h
 * Purpose:   Defines Application Class
 * Author:    runner.mei (runner.mei@gmail.com)
 * Created:   2008-10-31
 * Copyright: runner.mei ()
 * License:
 **************************************************************/

#ifndef JINXIAN_DOWNLOADERAPP_H
#define JINXIAN_DOWNLOADERAPP_H

#include <wx/app.h>
#include "wx/filesys.h"

class jxApp : public wxApp
{
    public:
        virtual bool OnInit();

	private:
	    wxFrame* m_frame;
};

DECLARE_APP(jxApp)

#endif // JINXIAN_DOWNLOADERAPP_H
