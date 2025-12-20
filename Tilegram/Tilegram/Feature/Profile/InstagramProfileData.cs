using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tilegram.Feature.Profile
{
    public class InstagramProfileService
    {
        private string UserName { get; set; }

        public InstagramProfileService(string userName)
        {
            UserName = userName;
        }

        public async Task<InstagramProfileData> GetProfileData()
        {
            await Task.Delay(1);

            var jsonExample = @"{
              'result': {
                'id': '25025320',
                'username': 'instagram',
                'is_private': false,
                'profile_pic_url': 'https://instagram.frix7-1.fna.fbcdn.net/v/t51.2885-19/550891366_18667771684001321_1383210656577177067_n.jpg?stp=dst-jpg_s150x150_tt6&efg=eyJ2ZW5jb2RlX3RhZyI6InByb2ZpbGVfcGljLmRqYW5nby4xMDgwLmMxIn0&_nc_ht=instagram.frix7-1.fna.fbcdn.net&_nc_cat=1&_nc_oc=Q6cZ2QEfbJQAem13W9-SWgUBwSvrAgol5-wlMpDS7IpRiSinc_39id35h4cyl8xu-QBf1Dc&_nc_ohc=kA4_1KXfSTUQ7kNvwFYGL3f&_nc_gid=zfq3u14l73QggNN-VsXGAg&edm=AOQ1c0wBAAAA&ccb=7-5&oh=00_AfmZ5BAtJ1q2D9UV2YSU1GFi6c-ISpojyrMNfZyzQtDpIg&oe=6949CCB1&_nc_sid=8b3546',
                'profile_pic_url_hd': 'https://instagram.frix7-1.fna.fbcdn.net/v/t51.2885-19/550891366_18667771684001321_1383210656577177067_n.jpg?stp=dst-jpg_s320x320_tt6&efg=eyJ2ZW5jb2RlX3RhZyI6InByb2ZpbGVfcGljLmRqYW5nby4xMDgwLmMxIn0&_nc_ht=instagram.frix7-1.fna.fbcdn.net&_nc_cat=1&_nc_oc=Q6cZ2QEfbJQAem13W9-SWgUBwSvrAgol5-wlMpDS7IpRiSinc_39id35h4cyl8xu-QBf1Dc&_nc_ohc=kA4_1KXfSTUQ7kNvwFYGL3f&_nc_gid=zfq3u14l73QggNN-VsXGAg&edm=AOQ1c0wBAAAA&ccb=7-5&oh=00_AfnTUDU3ndKMEzRMGbSELiKi8WgSsyrJD6uzahp8neV5aw&oe=6949CCB1&_nc_sid=8b3546',
                'biography': 'Discover whats new on Instagram 🔎✨',
                'full_name': 'Instagram',
                'edge_owner_to_timeline_media': {
                  'count': 8277
                },
                'edge_followed_by': {
                  'count': 698079524
                },
                'edge_follow': {
                  'count': 314
                },
                'profile_pic_url_wrapped': '/api/instagram/get?__sig=npNcrlkK_Ig2RJQMr3Nebg&__expires=1766130685&uri=https%3A%2F%2Finstagram.frix7-1.fna.fbcdn.net%2Fv%2Ft51.2885-19%2F550891366_18667771684001321_1383210656577177067_n.jpg%3Fstp%3Ddst-jpg_s150x150_tt6%26efg%3DeyJ2ZW5jb2RlX3RhZyI6InByb2ZpbGVfcGljLmRqYW5nby4xMDgwLmMxIn0%26_nc_ht%3Dinstagram.frix7-1.fna.fbcdn.net%26_nc_cat%3D1%26_nc_oc%3DQ6cZ2QEfbJQAem13W9-SWgUBwSvrAgol5-wlMpDS7IpRiSinc_39id35h4cyl8xu-QBf1Dc%26_nc_ohc%3DkA4_1KXfSTUQ7kNvwFYGL3f%26_nc_gid%3Dzfq3u14l73QggNN-VsXGAg%26edm%3DAOQ1c0wBAAAA%26ccb%3D7-5%26oh%3D00_AfmZ5BAtJ1q2D9UV2YSU1GFi6c-ISpojyrMNfZyzQtDpIg%26oe%3D6949CCB1%26_nc_sid%3D8b3546&filename=550891366_18667771684001321_1383210656577177067_n.jpg',
                'profile_pic_url_hd_wrapped': '/api/instagram/get?__sig=JQmoWvYbUcckh21zviYY6Q&__expires=1766130685&uri=https%3A%2F%2Finstagram.frix7-1.fna.fbcdn.net%2Fv%2Ft51.2885-19%2F550891366_18667771684001321_1383210656577177067_n.jpg%3Fstp%3Ddst-jpg_s320x320_tt6%26efg%3DeyJ2ZW5jb2RlX3RhZyI6InByb2ZpbGVfcGljLmRqYW5nby4xMDgwLmMxIn0%26_nc_ht%3Dinstagram.frix7-1.fna.fbcdn.net%26_nc_cat%3D1%26_nc_oc%3DQ6cZ2QEfbJQAem13W9-SWgUBwSvrAgol5-wlMpDS7IpRiSinc_39id35h4cyl8xu-QBf1Dc%26_nc_ohc%3DkA4_1KXfSTUQ7kNvwFYGL3f%26_nc_gid%3Dzfq3u14l73QggNN-VsXGAg%26edm%3DAOQ1c0wBAAAA%26ccb%3D7-5%26oh%3D00_AfnTUDU3ndKMEzRMGbSELiKi8WgSsyrJD6uzahp8neV5aw%26oe%3D6949CCB1%26_nc_sid%3D8b3546&filename=550891366_18667771684001321_1383210656577177067_n.jpg'
              }
            }";

            var jsonExampleResult = JsonConvert.DeserializeObject<InstagramProfileData.JsonResult>(jsonExample);

            var result = jsonExampleResult.Result;

            return new InstagramProfileData
            {
                Id = result.Id,
                Username = result.Username,
                FullName = result.FullName,
                Biography = result.Biography,
                ProfilePicUrl = result.ProfilePicUrl,
                ProfilePicUrlHd = result.ProfilePicUrlHd,
                PostsCount = result.EdgeOwnerToTimelineMedia.Count,
                FollowersCount = result.EdgeFollowedBy.Count,
                FollowingCount = result.EdgeFollow.Count,
                IsPrivate = result.IsPrivate
            };
        }

        public async Task<List<InstagramProfileHighlight>> LoadHightlights()
        {
            await Task.Delay(1);

            var jsonExample = @"{
              'result': [
                {
                  'id': 'highlight:18029499352961095',
                  'title': 'Meta AI ✍️',
                  'cover_media': {
                    'cropped_image_version': {
                      'url': 'https://scontent-nrt6-1.cdninstagram.com/v/t51.2885-15/572959565_683033951543541_80681017689541270_n.jpg?stp=c108.314.406.406a_dst-jpg_e15_s150x150_tt6&_nc_ht=scontent-nrt6-1.cdninstagram.com&_nc_cat=1&_nc_oc=Q6cZ2QHl4EgbbQln6AqfAEUJvLkc4a-8KdVG3I52SXg0b27eJN2g11MtN1RU3n_ShX7IyHE&_nc_ohc=O1T1jl4qVQYQ7kNvwE7vZWZ&_nc_gid=2k8AkAZgM1JUWKEoVm5bKQ&edm=AGW0Xe4BAAAA&ccb=7-5&oh=00_AflUQaKY_o-K-Ura3ag4nprxqRliu2gpk_SN2SHCQGi9Vg&oe=694AC87A&_nc_sid=94fea1',
                      'url_wrapped': '/api/instagram/get?__sig=bPv2sVdN8UFFlZdIBS24YQ&__expires=1766130989&uri=https%3A%2F%2Fscontent-nrt6-1.cdninstagram.com%2Fv%2Ft51.2885-15%2F572959565_683033951543541_80681017689541270_n.jpg%3Fstp%3Dc108.314.406.406a_dst-jpg_e15_s150x150_tt6%26_nc_ht%3Dscontent-nrt6-1.cdninstagram.com%26_nc_cat%3D1%26_nc_oc%3DQ6cZ2QHl4EgbbQln6AqfAEUJvLkc4a-8KdVG3I52SXg0b27eJN2g11MtN1RU3n_ShX7IyHE%26_nc_ohc%3DO1T1jl4qVQYQ7kNvwE7vZWZ%26_nc_gid%3D2k8AkAZgM1JUWKEoVm5bKQ%26edm%3DAGW0Xe4BAAAA%26ccb%3D7-5%26oh%3D00_AflUQaKY_o-K-Ura3ag4nprxqRliu2gpk_SN2SHCQGi9Vg%26oe%3D694AC87A%26_nc_sid%3D94fea1&filename=572959565_683033951543541_80681017689541270_n.jpg'
                    }
                  },
                  'user': {
                    'username': 'instagram',
                    'id': '25025320'
                  }
                },
                {
                  'id': 'highlight:18223279177302854',
                  'title': 'CFO Podcast',
                  'cover_media': {
                    'cropped_image_version': {
                      'url': 'https://scontent-nrt6-1.cdninstagram.com/v/t51.2885-15/551377207_662429250240179_6517642476883709504_n.jpg?stp=c95.396.320.320a_dst-jpg_e15_s150x150_tt6&_nc_ht=scontent-nrt6-1.cdninstagram.com&_nc_cat=1&_nc_oc=Q6cZ2QHl4EgbbQln6AqfAEUJvLkc4a-8KdVG3I52SXg0b27eJN2g11MtN1RU3n_ShX7IyHE&_nc_ohc=HyCyPG7k2j8Q7kNvwEqiIGc&_nc_gid=2k8AkAZgM1JUWKEoVm5bKQ&edm=AGW0Xe4BAAAA&ccb=7-5&oh=00_AfnNrtQiz5VSFKrMPXkcj2pGeFQD5txTvpg1qL7GYtconw&oe=694ACC40&_nc_sid=94fea1',
                      'url_wrapped': '/api/instagram/get?__sig=hM1Y90WZgZxAo_q-X1XjBA&__expires=1766130989&uri=https%3A%2F%2Fscontent-nrt6-1.cdninstagram.com%2Fv%2Ft51.2885-15%2F551377207_662429250240179_6517642476883709504_n.jpg%3Fstp%3Dc95.396.320.320a_dst-jpg_e15_s150x150_tt6%26_nc_ht%3Dscontent-nrt6-1.cdninstagram.com%26_nc_cat%3D1%26_nc_oc%3DQ6cZ2QHl4EgbbQln6AqfAEUJvLkc4a-8KdVG3I52SXg0b27eJN2g11MtN1RU3n_ShX7IyHE%26_nc_ohc%3DHyCyPG7k2j8Q7kNvwEqiIGc%26_nc_gid%3D2k8AkAZgM1JUWKEoVm5bKQ%26edm%3DAGW0Xe4BAAAA%26ccb%3D7-5%26oh%3D00_AfnNrtQiz5VSFKrMPXkcj2pGeFQD5txTvpg1qL7GYtconw%26oe%3D694ACC40%26_nc_sid%3D94fea1&filename=551377207_662429250240179_6517642476883709504_n.jpg'
                    }
                  },
                  'user': {
                    'username': 'instagram',
                    'id': '25025320'
                  }
                },
                {
                  'id': 'highlight:17914256252161608',
                  'title': 'IG Tips 📝',
                  'cover_media': {
                    'cropped_image_version': {
                      'url': 'https://scontent-nrt6-1.cdninstagram.com/v/t51.2885-15/563616145_18109328230514806_5475083639695792640_n.jpg?stp=c33.641.1080.1080a_dst-jpg_e15_s150x150_tt6&_nc_ht=scontent-nrt6-1.cdninstagram.com&_nc_cat=1&_nc_oc=Q6cZ2QHl4EgbbQln6AqfAEUJvLkc4a-8KdVG3I52SXg0b27eJN2g11MtN1RU3n_ShX7IyHE&_nc_ohc=Ajf8d3asAacQ7kNvwECrn7h&_nc_gid=2k8AkAZgM1JUWKEoVm5bKQ&edm=AGW0Xe4BAAAA&ccb=7-5&oh=00_AfkI_7yqcFHI091jPoQn3gpZsoL1ki2H4YSvR5h5S0NFSg&oe=694AC663&_nc_sid=94fea1',
                      'url_wrapped': '/api/instagram/get?__sig=Y9xhKPLr7gaLTuBnTp_UqQ&__expires=1766130989&uri=https%3A%2F%2Fscontent-nrt6-1.cdninstagram.com%2Fv%2Ft51.2885-15%2F563616145_18109328230514806_5475083639695792640_n.jpg%3Fstp%3Dc33.641.1080.1080a_dst-jpg_e15_s150x150_tt6%26_nc_ht%3Dscontent-nrt6-1.cdninstagram.com%26_nc_cat%3D1%26_nc_oc%3DQ6cZ2QHl4EgbbQln6AqfAEUJvLkc4a-8KdVG3I52SXg0b27eJN2g11MtN1RU3n_ShX7IyHE%26_nc_ohc%3DAjf8d3asAacQ7kNvwECrn7h%26_nc_gid%3D2k8AkAZgM1JUWKEoVm5bKQ%26edm%3DAGW0Xe4BAAAA%26ccb%3D7-5%26oh%3D00_AfkI_7yqcFHI091jPoQn3gpZsoL1ki2H4YSvR5h5S0NFSg%26oe%3D694AC663%26_nc_sid%3D94fea1&filename=563616145_18109328230514806_5475083639695792640_n.jpg'
                    }
                  },
                  'user': {
                    'username': 'instagram',
                    'id': '25025320'
                  }
                },
                {
                  'id': 'highlight:18065022955842095',
                  'title': '✨✨✨',
                  'cover_media': {
                    'cropped_image_version': {
                      'url': 'https://scontent-nrt6-1.cdninstagram.com/v/t51.2885-15/474717014_8763729123755962_1477618173007457264_n.jpg?stp=c0.247.640.640a_dst-jpg_e15_s150x150_tt6&_nc_ht=scontent-nrt6-1.cdninstagram.com&_nc_cat=1&_nc_oc=Q6cZ2QHl4EgbbQln6AqfAEUJvLkc4a-8KdVG3I52SXg0b27eJN2g11MtN1RU3n_ShX7IyHE&_nc_ohc=qIJ8VsXijhMQ7kNvwH78pom&_nc_gid=2k8AkAZgM1JUWKEoVm5bKQ&edm=AGW0Xe4BAAAA&ccb=7-5&oh=00_AfnFWiRAM2Go0dwopQFMQo94Sj8AmBgdGPtoLa0iTIyzsw&oe=694ABB42&_nc_sid=94fea1',
                      'url_wrapped': '/api/instagram/get?__sig=T4EfabQDEI1x2JcJbw4T8g&__expires=1766130989&uri=https%3A%2F%2Fscontent-nrt6-1.cdninstagram.com%2Fv%2Ft51.2885-15%2F474717014_8763729123755962_1477618173007457264_n.jpg%3Fstp%3Dc0.247.640.640a_dst-jpg_e15_s150x150_tt6%26_nc_ht%3Dscontent-nrt6-1.cdninstagram.com%26_nc_cat%3D1%26_nc_oc%3DQ6cZ2QHl4EgbbQln6AqfAEUJvLkc4a-8KdVG3I52SXg0b27eJN2g11MtN1RU3n_ShX7IyHE%26_nc_ohc%3DqIJ8VsXijhMQ7kNvwH78pom%26_nc_gid%3D2k8AkAZgM1JUWKEoVm5bKQ%26edm%3DAGW0Xe4BAAAA%26ccb%3D7-5%26oh%3D00_AfnFWiRAM2Go0dwopQFMQo94Sj8AmBgdGPtoLa0iTIyzsw%26oe%3D694ABB42%26_nc_sid%3D94fea1&filename=474717014_8763729123755962_1477618173007457264_n.jpg'
                    }
                  },
                  'user': {
                    'username': 'instagram',
                    'id': '25025320'
                  }
                },
                {
                  'id': 'highlight:18060386815694422',
                  'title': 'Halloween 2024',
                  'cover_media': {
                    'cropped_image_version': {
                      'url': 'https://scontent-nrt6-1.cdninstagram.com/v/t51.2885-15/563791088_779849101687566_3876016035141090288_n.jpg?stp=c0.248.640.640a_dst-jpg_e15_s150x150_tt6&_nc_ht=scontent-nrt6-1.cdninstagram.com&_nc_cat=1&_nc_oc=Q6cZ2QHl4EgbbQln6AqfAEUJvLkc4a-8KdVG3I52SXg0b27eJN2g11MtN1RU3n_ShX7IyHE&_nc_ohc=v63uwIfmz_4Q7kNvwGVWL6m&_nc_gid=2k8AkAZgM1JUWKEoVm5bKQ&edm=AGW0Xe4BAAAA&ccb=7-5&oh=00_AflE7XbmI42lwdRQo5lbPwIr-VyhybqFYChbh7EgVTs7Lg&oe=694AB88F&_nc_sid=94fea1',
                      'url_wrapped': '/api/instagram/get?__sig=vdnSbK4k4Xi1Y4hPy661bQ&__expires=1766130989&uri=https%3A%2F%2Fscontent-nrt6-1.cdninstagram.com%2Fv%2Ft51.2885-15%2F563791088_779849101687566_3876016035141090288_n.jpg%3Fstp%3Dc0.248.640.640a_dst-jpg_e15_s150x150_tt6%26_nc_ht%3Dscontent-nrt6-1.cdninstagram.com%26_nc_cat%3D1%26_nc_oc%3DQ6cZ2QHl4EgbbQln6AqfAEUJvLkc4a-8KdVG3I52SXg0b27eJN2g11MtN1RU3n_ShX7IyHE%26_nc_ohc%3Dv63uwIfmz_4Q7kNvwGVWL6m%26_nc_gid%3D2k8AkAZgM1JUWKEoVm5bKQ%26edm%3DAGW0Xe4BAAAA%26ccb%3D7-5%26oh%3D00_AflE7XbmI42lwdRQo5lbPwIr-VyhybqFYChbh7EgVTs7Lg%26oe%3D694AB88F%26_nc_sid%3D94fea1&filename=563791088_779849101687566_3876016035141090288_n.jpg'
                    }
                  },
                  'user': {
                    'username': 'instagram',
                    'id': '25025320'
                  }
                },
                {
                  'id': 'highlight:17951474176899044',
                  'title': 'Creatives ✩',
                  'cover_media': {
                    'cropped_image_version': {
                      'url': 'https://scontent-nrt6-1.cdninstagram.com/v/t51.2885-15/570618028_1548661276146282_6798913251273740107_n.jpg?stp=c32.196.590.590a_dst-jpg_e15_s150x150_tt6&_nc_ht=scontent-nrt6-1.cdninstagram.com&_nc_cat=106&_nc_oc=Q6cZ2QHl4EgbbQln6AqfAEUJvLkc4a-8KdVG3I52SXg0b27eJN2g11MtN1RU3n_ShX7IyHE&_nc_ohc=ryzkEkxueesQ7kNvwEDbUAp&_nc_gid=2k8AkAZgM1JUWKEoVm5bKQ&edm=AGW0Xe4BAAAA&ccb=7-5&oh=00_Afl49xlTuF9vpKkZapXXMqbv0_13_7lkVIjR8Fy_1jONPg&oe=694AE4CE&_nc_sid=94fea1',
                      'url_wrapped': '/api/instagram/get?__sig=81Qnez-uwvs9gE5ZKXi6vQ&__expires=1766130989&uri=https%3A%2F%2Fscontent-nrt6-1.cdninstagram.com%2Fv%2Ft51.2885-15%2F570618028_1548661276146282_6798913251273740107_n.jpg%3Fstp%3Dc32.196.590.590a_dst-jpg_e15_s150x150_tt6%26_nc_ht%3Dscontent-nrt6-1.cdninstagram.com%26_nc_cat%3D106%26_nc_oc%3DQ6cZ2QHl4EgbbQln6AqfAEUJvLkc4a-8KdVG3I52SXg0b27eJN2g11MtN1RU3n_ShX7IyHE%26_nc_ohc%3DryzkEkxueesQ7kNvwEDbUAp%26_nc_gid%3D2k8AkAZgM1JUWKEoVm5bKQ%26edm%3DAGW0Xe4BAAAA%26ccb%3D7-5%26oh%3D00_Afl49xlTuF9vpKkZapXXMqbv0_13_7lkVIjR8Fy_1jONPg%26oe%3D694AE4CE%26_nc_sid%3D94fea1&filename=570618028_1548661276146282_6798913251273740107_n.jpg'
                    }
                  },
                  'user': {
                    'username': 'instagram',
                    'id': '25025320'
                  }
                },
                {
                  'id': 'highlight:17931388058310575',
                  'title': 'Hello World',
                  'cover_media': {
                    'cropped_image_version': {
                      'url': 'https://scontent-nrt6-1.cdninstagram.com/v/t51.2885-15/559807961_1888367778774318_4471467396728274454_n.jpg?stp=c19.288.589.589a_dst-jpg_e15_s150x150_tt6&_nc_ht=scontent-nrt6-1.cdninstagram.com&_nc_cat=106&_nc_oc=Q6cZ2QHl4EgbbQln6AqfAEUJvLkc4a-8KdVG3I52SXg0b27eJN2g11MtN1RU3n_ShX7IyHE&_nc_ohc=fwKkDufpqzcQ7kNvwH-OK4y&_nc_gid=2k8AkAZgM1JUWKEoVm5bKQ&edm=AGW0Xe4BAAAA&ccb=7-5&oh=00_AfkTGqCSMYlWa-ZrvJ8FJkwy3H9s87SBA226DjxDWEYJRw&oe=694AD5F3&_nc_sid=94fea1',
                      'url_wrapped': '/api/instagram/get?__sig=wATYFpoqtLzNCe6qnChwKw&__expires=1766130989&uri=https%3A%2F%2Fscontent-nrt6-1.cdninstagram.com%2Fv%2Ft51.2885-15%2F559807961_1888367778774318_4471467396728274454_n.jpg%3Fstp%3Dc19.288.589.589a_dst-jpg_e15_s150x150_tt6%26_nc_ht%3Dscontent-nrt6-1.cdninstagram.com%26_nc_cat%3D106%26_nc_oc%3DQ6cZ2QHl4EgbbQln6AqfAEUJvLkc4a-8KdVG3I52SXg0b27eJN2g11MtN1RU3n_ShX7IyHE%26_nc_ohc%3DfwKkDufpqzcQ7kNvwH-OK4y%26_nc_gid%3D2k8AkAZgM1JUWKEoVm5bKQ%26edm%3DAGW0Xe4BAAAA%26ccb%3D7-5%26oh%3D00_AfkTGqCSMYlWa-ZrvJ8FJkwy3H9s87SBA226DjxDWEYJRw%26oe%3D694AD5F3%26_nc_sid%3D94fea1&filename=559807961_1888367778774318_4471467396728274454_n.jpg'
                    }
                  },
                  'user': {
                    'username': 'instagram',
                    'id': '25025320'
                  }
                },
                {
                  'id': 'highlight:18025342139305098',
                  'title': 'Too Reel',
                  'cover_media': {
                    'cropped_image_version': {
                      'url': 'https://scontent-nrt6-1.cdninstagram.com/v/t51.2885-15/569130362_1226982555931098_5470193290509326986_n.jpg?stp=c102.357.430.430a_dst-jpg_e15_s150x150_tt6&_nc_ht=scontent-nrt6-1.cdninstagram.com&_nc_cat=106&_nc_oc=Q6cZ2QHl4EgbbQln6AqfAEUJvLkc4a-8KdVG3I52SXg0b27eJN2g11MtN1RU3n_ShX7IyHE&_nc_ohc=2QrvVx1sHVgQ7kNvwH0kmju&_nc_gid=2k8AkAZgM1JUWKEoVm5bKQ&edm=AGW0Xe4BAAAA&ccb=7-5&oh=00_AfkZ_JwEKaBuh-vJ1i1P_iMPZWYYr-CyKBAxzod326NKQQ&oe=694AE491&_nc_sid=94fea1',
                      'url_wrapped': '/api/instagram/get?__sig=O9paw_np0Cq-cvfpknt8GA&__expires=1766130989&uri=https%3A%2F%2Fscontent-nrt6-1.cdninstagram.com%2Fv%2Ft51.2885-15%2F569130362_1226982555931098_5470193290509326986_n.jpg%3Fstp%3Dc102.357.430.430a_dst-jpg_e15_s150x150_tt6%26_nc_ht%3Dscontent-nrt6-1.cdninstagram.com%26_nc_cat%3D106%26_nc_oc%3DQ6cZ2QHl4EgbbQln6AqfAEUJvLkc4a-8KdVG3I52SXg0b27eJN2g11MtN1RU3n_ShX7IyHE%26_nc_ohc%3D2QrvVx1sHVgQ7kNvwH0kmju%26_nc_gid%3D2k8AkAZgM1JUWKEoVm5bKQ%26edm%3DAGW0Xe4BAAAA%26ccb%3D7-5%26oh%3D00_AfkZ_JwEKaBuh-vJ1i1P_iMPZWYYr-CyKBAxzod326NKQQ%26oe%3D694AE491%26_nc_sid%3D94fea1&filename=569130362_1226982555931098_5470193290509326986_n.jpg'
                    }
                  },
                  'user': {
                    'username': 'instagram',
                    'id': '25025320'
                  }
                },
                {
                  'id': 'highlight:18040272811712584',
                  'title': '24for24',
                  'cover_media': {
                    'cropped_image_version': {
                      'url': 'https://scontent-nrt1-2.cdninstagram.com/v/t51.2885-15/422906841_3656118291301501_8814744082897606552_n.jpg?stp=dst-jpg_s150x150_tt6&_nc_ht=scontent-nrt1-2.cdninstagram.com&_nc_cat=101&_nc_oc=Q6cZ2QHl4EgbbQln6AqfAEUJvLkc4a-8KdVG3I52SXg0b27eJN2g11MtN1RU3n_ShX7IyHE&_nc_ohc=A4srnFh92PEQ7kNvwHjc0Sk&_nc_gid=2k8AkAZgM1JUWKEoVm5bKQ&edm=AGW0Xe4BAAAA&ccb=7-5&oh=00_Aflx5i-BMdOi3bqscS7drV5ElMheNkuyDdLX78ihrWs2VA&oe=694ACF43&_nc_sid=94fea1',
                      'url_wrapped': '/api/instagram/get?__sig=29vrISve2Xi-BKRY89mqKQ&__expires=1766130989&uri=https%3A%2F%2Fscontent-nrt1-2.cdninstagram.com%2Fv%2Ft51.2885-15%2F422906841_3656118291301501_8814744082897606552_n.jpg%3Fstp%3Ddst-jpg_s150x150_tt6%26_nc_ht%3Dscontent-nrt1-2.cdninstagram.com%26_nc_cat%3D101%26_nc_oc%3DQ6cZ2QHl4EgbbQln6AqfAEUJvLkc4a-8KdVG3I52SXg0b27eJN2g11MtN1RU3n_ShX7IyHE%26_nc_ohc%3DA4srnFh92PEQ7kNvwHjc0Sk%26_nc_gid%3D2k8AkAZgM1JUWKEoVm5bKQ%26edm%3DAGW0Xe4BAAAA%26ccb%3D7-5%26oh%3D00_Aflx5i-BMdOi3bqscS7drV5ElMheNkuyDdLX78ihrWs2VA%26oe%3D694ACF43%26_nc_sid%3D94fea1&filename=422906841_3656118291301501_8814744082897606552_n.jpg'
                    }
                  },
                  'user': {
                    'username': 'instagram',
                    'id': '25025320'
                  }
                },
                {
                  'id': 'highlight:17983407089364361',
                  'title': 'Trend Talk 2024',
                  'cover_media': {
                    'cropped_image_version': {
                      'url': 'https://scontent-nrt1-2.cdninstagram.com/v/t51.2885-15/582411248_18085053622950745_4949473843365105691_n.jpg?stp=c0.458.1179.1179a_dst-jpg_e15_s150x150_tt6&_nc_ht=scontent-nrt1-2.cdninstagram.com&_nc_cat=101&_nc_oc=Q6cZ2QHl4EgbbQln6AqfAEUJvLkc4a-8KdVG3I52SXg0b27eJN2g11MtN1RU3n_ShX7IyHE&_nc_ohc=QeOivJToUWQQ7kNvwGOC3py&_nc_gid=2k8AkAZgM1JUWKEoVm5bKQ&edm=AGW0Xe4BAAAA&ccb=7-5&oh=00_AfmRIwDFRmcARhuC_yBXECU9msW_SRwPaJsxHbrTZeo5oQ&oe=694AD36A&_nc_sid=94fea1',
                      'url_wrapped': '/api/instagram/get?__sig=HgyuYeo3Htlr3GXEoHLkrg&__expires=1766130989&uri=https%3A%2F%2Fscontent-nrt1-2.cdninstagram.com%2Fv%2Ft51.2885-15%2F582411248_18085053622950745_4949473843365105691_n.jpg%3Fstp%3Dc0.458.1179.1179a_dst-jpg_e15_s150x150_tt6%26_nc_ht%3Dscontent-nrt1-2.cdninstagram.com%26_nc_cat%3D101%26_nc_oc%3DQ6cZ2QHl4EgbbQln6AqfAEUJvLkc4a-8KdVG3I52SXg0b27eJN2g11MtN1RU3n_ShX7IyHE%26_nc_ohc%3DQeOivJToUWQQ7kNvwGOC3py%26_nc_gid%3D2k8AkAZgM1JUWKEoVm5bKQ%26edm%3DAGW0Xe4BAAAA%26ccb%3D7-5%26oh%3D00_AfmRIwDFRmcARhuC_yBXECU9msW_SRwPaJsxHbrTZeo5oQ%26oe%3D694AD36A%26_nc_sid%3D94fea1&filename=582411248_18085053622950745_4949473843365105691_n.jpg'
                    }
                  },
                  'user': {
                    'username': 'instagram',
                    'id': '25025320'
                  }
                },
                {
                  'id': 'highlight:17947529159342449',
                  'title': 'FutureMakers',
                  'cover_media': {
                    'cropped_image_version': {
                      'url': 'https://scontent-nrt6-1.cdninstagram.com/v/t51.2885-15/558980553_811388094722129_5838493335769046813_n.jpg?stp=c99.329.434.434a_dst-jpg_e15_s150x150_tt6&_nc_ht=scontent-nrt6-1.cdninstagram.com&_nc_cat=105&_nc_oc=Q6cZ2QHl4EgbbQln6AqfAEUJvLkc4a-8KdVG3I52SXg0b27eJN2g11MtN1RU3n_ShX7IyHE&_nc_ohc=YaY7i3h9H50Q7kNvwGMMb69&_nc_gid=2k8AkAZgM1JUWKEoVm5bKQ&edm=AGW0Xe4BAAAA&ccb=7-5&oh=00_AfmrxQBsTx6UW0i6AVbk4A9tdHLrzFKnjXVTLmTF34KM4g&oe=694AE8C6&_nc_sid=94fea1',
                      'url_wrapped': '/api/instagram/get?__sig=75MF7P3XSYD5dB5JR5sNFg&__expires=1766130989&uri=https%3A%2F%2Fscontent-nrt6-1.cdninstagram.com%2Fv%2Ft51.2885-15%2F558980553_811388094722129_5838493335769046813_n.jpg%3Fstp%3Dc99.329.434.434a_dst-jpg_e15_s150x150_tt6%26_nc_ht%3Dscontent-nrt6-1.cdninstagram.com%26_nc_cat%3D105%26_nc_oc%3DQ6cZ2QHl4EgbbQln6AqfAEUJvLkc4a-8KdVG3I52SXg0b27eJN2g11MtN1RU3n_ShX7IyHE%26_nc_ohc%3DYaY7i3h9H50Q7kNvwGMMb69%26_nc_gid%3D2k8AkAZgM1JUWKEoVm5bKQ%26edm%3DAGW0Xe4BAAAA%26ccb%3D7-5%26oh%3D00_AfmrxQBsTx6UW0i6AVbk4A9tdHLrzFKnjXVTLmTF34KM4g%26oe%3D694AE8C6%26_nc_sid%3D94fea1&filename=558980553_811388094722129_5838493335769046813_n.jpg'
                    }
                  },
                  'user': {
                    'username': 'instagram',
                    'id': '25025320'
                  }
                }
              ]
            }";

            var jsonExampleResult = JsonConvert.DeserializeObject<InstagramProfileHighlight.Root>(jsonExample);

            return jsonExampleResult.Result.Select(s => new InstagramProfileHighlight
            {
                Id = s.Id,
                Title = s.Title,
                CoverUrl = s.CoverMedia.CroppedImageVersion.Url,
                Username = s.User.Username
            }).ToList();
        }


        public class InstagramProfileHighlight
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string CoverUrl { get; set; }
            public string Username { get; set; }

            public class CoverMedia
            {
                [JsonProperty("cropped_image_version")]
                public CroppedImageVersion CroppedImageVersion { get; set; }
            }

            public class CroppedImageVersion
            {
                [JsonProperty("url")]
                public string Url { get; set; }

                [JsonProperty("url_wrapped")]
                public string UrlWrapped { get; set; }
            }

            public class Result
            {
                [JsonProperty("id")]
                public string Id { get; set; }

                [JsonProperty("title")]
                public string Title { get; set; }

                [JsonProperty("cover_media")]
                public CoverMedia CoverMedia { get; set; }

                [JsonProperty("user")]
                public User User { get; set; }
            }

            public class Root
            {
                [JsonProperty("result")]
                public List<Result> Result { get; set; }
            }

            public class User
            {
                [JsonProperty("username")]
                public string Username { get; set; }

                [JsonProperty("id")]
                public string Id { get; set; }
            }
        }

        // DTO para la UI
        public class InstagramProfileData
        {
            public string Id { get; set; }
            public string Username { get; set; }
            public string FullName { get; set; }
            public string Biography { get; set; }
            public string ProfilePicUrl { get; set; }
            public string ProfilePicUrlHd { get; set; }
            public int PostsCount { get; set; }
            public int FollowersCount { get; set; }
            public int FollowingCount { get; set; }
            public bool IsPrivate { get; set; }

            // Clases auxiliares para deserialización
            public class EdgeFollow
            {
                [JsonProperty("count")]
                public int Count { get; set; }
            }

            public class EdgeFollowedBy
            {
                [JsonProperty("count")]
                public int Count { get; set; }
            }

            public class EdgeOwnerToTimelineMedia
            {
                [JsonProperty("count")]
                public int Count { get; set; }
            }

            public class Result
            {
                [JsonProperty("id")]
                public string Id { get; set; }

                [JsonProperty("username")]
                public string Username { get; set; }

                [JsonProperty("is_private")]
                public bool IsPrivate { get; set; }

                [JsonProperty("profile_pic_url")]
                public string ProfilePicUrl { get; set; }

                [JsonProperty("profile_pic_url_hd")]
                public string ProfilePicUrlHd { get; set; }

                [JsonProperty("biography")]
                public string Biography { get; set; }

                [JsonProperty("full_name")]
                public string FullName { get; set; }

                [JsonProperty("edge_owner_to_timeline_media")]
                public EdgeOwnerToTimelineMedia EdgeOwnerToTimelineMedia { get; set; }

                [JsonProperty("edge_followed_by")]
                public EdgeFollowedBy EdgeFollowedBy { get; set; }

                [JsonProperty("edge_follow")]
                public EdgeFollow EdgeFollow { get; set; }

                [JsonProperty("profile_pic_url_wrapped")]
                public string ProfilePicUrlWrapped { get; set; }

                [JsonProperty("profile_pic_url_hd_wrapped")]
                public string ProfilePicUrlHdWrapped { get; set; }
            }

            public class JsonResult
            {
                [JsonProperty("result")]
                public Result Result { get; set; }
            }
        }
    }

}
