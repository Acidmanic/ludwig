using Ludwig.Common.Download;
using Ludwig.Common.Extensions;
using Ludwig.IssueManager.Jira.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Ludwig.IssueManager.Jira.Services
{
    public class DevelopmentMockCookieForwarder : ICookieForwarder
    {
        private const string TestCookies =
            "remember_user_token=eyJfcmFpbHMiOnsibWVzc2FnZSI6Ilcxc3lYU3dpSkRKaEpERXdKRzh2UjNoeFdYaEdSR1V4VmtzNVYzQTNSalJZYUdVaUxDSXhOalkzT1RBMU1qVXlMamd4TWpRek5TSmQiLCJleHAiOiIyMDIyLTExLTIyVDExOjAwOjUyLjgxMloiLCJwdXIiOiJjb29raWUucmVtZW1iZXJfdXNlcl90b2tlbiJ9fQ%3D%3D--810fe92e545a827f0e0945bf4d7adf6ffa4f11db; known_sign_in=R0VNQmMwV0ZwT2tmU0c4cVdaUk84cWhGZE1aR2VlWTl1Y0MzZmI2S2l0djlvc3dZQXVhbWlMK3ppeWlaK2YyV0s3U2lvYW1EalhpaCs1NHhKYllBNzU1TEZkbTJGMHlKUnlvb1dCaU4rWXFXRU4ySTVyQjBQZ3ZQUUZqSDZnbDYtLXVVN1VQZEdaOTM3RjZadFEzWGxtb0E9PQ%3D%3D--5c705960f6ef066331fe67c5706d21e6001243b8; _gitlab_session=6294f01af4a5d96ee269538141418548; event_filter=all; sidebar_collapsed=false; hide_auto_devops_implicitly_enabled_banner_7=false; seraph.rememberme.cookie=10704%3A720bdd7a0f503a6ac9ec8e30b18397c807343116; atlassian.xsrf.token=BKCN-INSJ-C3I1-ALDF_7aa8880b34bd3fd3f583549c3c9338a1bbc1ea9b_lin; jira.editor.user.mode=wysiwyg; JSESSIONID=35902381F93127469D0D0FC49DB85A2B";


        public void ForwardCookies(HttpContext context, PatientDownloader downloader)
        {
            downloader.InDirectCookies = TestCookies.LoadCookies();
        }
    }
}