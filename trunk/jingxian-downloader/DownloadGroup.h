/***************************************************************
 * Name:      DownloadGroup.h
 * Purpose:   Defines DownloadGroup Class
 * Author:    runner.mei (runner.mei@gmail.com)
 * Created:   2008-11-15
 * Copyright: runner.mei
 * License:
 **************************************************************/

#ifndef DOWNLOAD_GROUP_H
#define DOWNLOAD_GROUP_H

#include <wx/string.h>
#include <wx/list.h>

class DownloadGroup;

WX_DECLARE_LIST(DownloadGroup, DownloadGroupList);

class DownloadGroup : wxObject
{
public:
	DownloadGroup();
	DownloadGroup(const wxChar* nm, const wxChar* folder);
	DownloadGroup(const wxChar* nm, const wxChar* folder, const wxStringList& exts);
	~DownloadGroup(void);

	const wxString& Name() const;
	void Name(const wxString& nm);

	const wxString& Folder() const;
	void Folder(const wxString& folder);

	wxStringList& Exts();
	const wxStringList& Exts() const;


	DownloadGroupList& Childs();
	const DownloadGroupList& Childs() const;

    DECLARE_DYNAMIC_CLASS(DownloadGroup);
private:
	
	wxString m_name;
	wxString m_folder;
	wxStringList m_exts;

	DownloadGroupList m_childs;
};

#endif // DOWNLOAD_GROUP_H