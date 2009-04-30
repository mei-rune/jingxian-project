/***************************************************************
 * Name:      DownloadGroup.cpp
 * Purpose:   Code for DownloadGroup Class
 * Author:    runner.mei (runner.mei@gmail.com)
 * Created:   2008-11-15
 * Copyright: runner.mei
 * License:
 **************************************************************/

#include "pre_config.h"
#include <wx/listimpl.cpp>
#include "DownloadGroup.h"


IMPLEMENT_DYNAMIC_CLASS(DownloadGroup, wxObject);
WX_DEFINE_LIST(DownloadGroupList);

DownloadGroup::DownloadGroup(void)
{
}

DownloadGroup::DownloadGroup(const wxChar* nm, const wxChar* folder)
{
	wxASSERT( nm );
	wxASSERT( folder );

	m_name = nm;
	m_folder = folder;
}

DownloadGroup::DownloadGroup(const wxChar* nm, const wxChar* folder, const wxStringList& exts)
: m_exts( exts )
{
	wxASSERT( nm );
	wxASSERT( folder );

	m_name = nm;
	m_folder = folder;
}

DownloadGroup::~DownloadGroup(void)
{
#if !wxUSE_STL
        m_childs.DeleteContents(true);
#else
        WX_CLEAR_LIST(DownloadGroupList, m_childs);
#endif
}

const wxString& DownloadGroup::Name() const
{
	return m_name;
}

void DownloadGroup::Name(const wxString& nm)
{
	m_name = nm;
}

const wxString& DownloadGroup::Folder() const
{
	return m_folder;
}

void DownloadGroup::Folder(const wxString& folder)
{
	m_folder = folder;
}

wxStringList& DownloadGroup::Exts()
{
	return m_exts;
}

const wxStringList& DownloadGroup::Exts() const
{
	return m_exts;
}


DownloadGroupList& DownloadGroup::Childs()
{
	return m_childs;
}

const DownloadGroupList& DownloadGroup::Childs() const
{
	return m_childs;
}