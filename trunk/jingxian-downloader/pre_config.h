/***************************************************************
 * Name:      wx_pch.h
 * Purpose:   Header to create Pre-Compiled Header (PCH)
 * Author:    runner.mei (runner.mei@gmail.com)
 * Created:   2008-10-31
 * Copyright: runner.mei ()
 * License:   
 **************************************************************/

#ifndef PRE_CONFIG_H_INCLUDED
#define PRE_CONFIG_H_INCLUDED

#ifndef __WXMSW__
#define __WXMSW__
#endif

#ifdef _DEBUG
#ifndef __WXDEBUG__
#define __WXDEBUG__ // Add this for debug builds that link to wxWidgets debug configurations. Activates wxASSERT(), etc. See the Debugging Overview for more information.
#endif
#endif

#define _CRT_SECURE_NO_DEPRECATE // Add this if you want to suppress "This function or variable may be unsafe. Consider using <alternative> instead." warnings.
#define _CRT_NONSTDC_NO_DEPRECATE // Suppresses other warnings.

#ifndef CURL_STATICLIB
#define CURL_STATICLIB
#endif

#include <winsock2.h>

// basic wxWidgets headers
#include <wx/wxprec.h>

#ifndef WX_PRECOMP
#include "wx/wx.h"
#endif

#endif // PRE_CONFIG_H_INCLUDED
