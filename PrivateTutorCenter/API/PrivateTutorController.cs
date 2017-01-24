using PrivateTutorCenter.Models;
using PrivateTutorCenter.Models.View_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace PrivateTutorCenter.API
{

    public class PrivateTutorController : ApiController
    {
        public List<EditableCourse> Get([FromUri]string selectedCategory)
        {
            string userId = null;
            bool isStudent = false;
            bool isTeacher = false;
            bool isAdmin = false;
            bool isAnynomous = false;

            if (User.Identity.IsAuthenticated)
            {
                userId = ((ClaimsPrincipal)this.User).FindFirst(ClaimTypes.NameIdentifier).Value;
                isStudent = this.User.IsInRole("Student");
                isTeacher = this.User.IsInRole("Teacher");
                isAdmin = this.User.IsInRole("Admin");
            } else
            {
                isAnynomous = true;
            }


            if (isStudent)
            {
                if (HttpContext.Current.Cache["Student" + "-Category-" + selectedCategory] != null)
                    return (List<EditableCourse>)HttpContext.Current.Cache["Student" + "-Category-" + selectedCategory];
            }

            else if (isTeacher)
            {
                if (HttpContext.Current.Cache["Teacher" + "-Category-" + selectedCategory] != null)
                    return (List<EditableCourse>)HttpContext.Current.Cache["Teacher" + "-Category-" + selectedCategory];
            }

            else if (isAdmin)
            {
                if (HttpContext.Current.Cache["Admin" + "-Category-" + selectedCategory] != null)
                    return (List<EditableCourse>)HttpContext.Current.Cache["Admin" + "-Category-" + selectedCategory];
            }
            else
            {
                if (HttpContext.Current.Cache["Anonymous" + "-Category-" + selectedCategory] != null)
                    return (List<EditableCourse>)HttpContext.Current.Cache["Anonymous" + "-Category-" + selectedCategory];
            }

            using (PrivateTutorContext context = new PrivateTutorContext())
            {
                var courses = context.courses.Select(p => new EditableCourse { id = p.id, title = p.title, description = p.description, subject = p.subject, teacherId=p.teacherId, isEditable=isAdmin, canEnroll=isStudent||isAnynomous }).ToList();
                List<EditableCourse> result = new List<EditableCourse>();

                if (selectedCategory == "getAll")
                {
                    foreach (var item in courses)
                    {
                            if (isTeacher)
                            {
                                if (item.teacherId == userId)
                                    item.isEditable = true;
                            }
                            result.Add(item);
                    }
                    result = courses;
                }
                else
                {
                    foreach (var item in courses)
                    {
                        if (item.subject == selectedCategory)
                        {
                            if (isTeacher)
                            {
                                if (item.teacherId == userId)
                                    item.isEditable = true;
                            }
                            result.Add(item);
                        }
                    }
                }

                if (isStudent)
                {
                    HttpContext.Current.Cache["Student" + "-Category-" + selectedCategory] = result;
                }
                else if (isTeacher)
                {
                    HttpContext.Current.Cache["Teacher" + "-Category-" + selectedCategory] = result;
                }
                else if (isAdmin)
                {
                    HttpContext.Current.Cache["Admin" + "-Category-" + selectedCategory] = result;
                } else
                {
                    HttpContext.Current.Cache["Anonymous" + "-Category-" + selectedCategory] = result;
                }

                return result;
            }
        }

        [Authorize]
        public EditableCourse Get(int id)
        {
            var userId = ((ClaimsPrincipal)this.User).FindFirst(ClaimTypes.NameIdentifier).Value;

            if (HttpContext.Current.Cache["Item-" + id] != null)
                return (EditableCourse)HttpContext.Current.Cache["Item-" + id];
            using (PrivateTutorContext context = new PrivateTutorContext())
            {
                var product = context.courses.Find(id);
                EditableCourse eItem = new EditableCourse { id = product.id, title = product.title, teacherId = userId, description = product.description, subject = product.subject };
                HttpContext.Current.Cache["Item-" + id] = eItem;
                return eItem;
            }
        }

        [Authorize]
        public HttpResponseMessage Post([FromBody]Course pCourse)
        {
            var userId = ((ClaimsPrincipal)this.User).FindFirst(ClaimTypes.NameIdentifier).Value;
            HttpContext.Current.Cache.Remove("Student"+"-Category-" + pCourse.subject);
            HttpContext.Current.Cache.Remove("Teacher" + "-Category-" + pCourse.subject);
            HttpContext.Current.Cache.Remove("Admin" + "-Category-" + pCourse.subject);
            HttpContext.Current.Cache.Remove("Anonymous" + "-Category-" + pCourse.subject);

            HttpContext.Current.Cache.Remove("Student" + "-Category-" + "getAll");
            HttpContext.Current.Cache.Remove("Teacher" + "-Category-" + "getAll");
            HttpContext.Current.Cache.Remove("Admin" + "-Category-" + "getAll");
            HttpContext.Current.Cache.Remove("Anonymous" + "-Category-" + "getAll");

            using (PrivateTutorContext context = new PrivateTutorContext())
            {
                if (pCourse.id == 0)
                {
                    pCourse.teacherId = userId;
                    context.courses.Add(pCourse);
                    context.SaveChanges();
                    EditableCourse eItem = new EditableCourse { id = pCourse.id, title = pCourse.title, teacherId = userId, subject = pCourse.subject, description = pCourse.description };
                    HttpContext.Current.Cache["Item-" + eItem.id] = eItem;
                    return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "Course Added." });
                }
                else
                {
                    var course = context.courses.Find(pCourse.id);
                    if (User.IsInRole("Admin") || (course.teacherId == userId))
                    {
                        course.title = pCourse.title;
                        course.description = pCourse.description;
                        course.subject = pCourse.subject;
                        context.SaveChanges();
                        EditableCourse eItem = new EditableCourse { id = pCourse.id, title = pCourse.title, teacherId = userId, subject = pCourse.subject, description = pCourse.description };
                        HttpContext.Current.Cache["Item-" + eItem.id] = eItem;
                        return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "Course Updated" });
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { success = false, message = "Course is not Updated. Not Authorized" });
                }
            }
        }

        [Authorize]
        public HttpResponseMessage Delete(int id)
        {
            var userId = ((ClaimsPrincipal)this.User).FindFirst(ClaimTypes.NameIdentifier).Value;
            using (PrivateTutorContext context = new PrivateTutorContext())
            {
                var course = context.courses.Find(id);
                if (User.IsInRole("Admin") || (course.teacherId == userId))
                {
                    context.courses.Remove(course);
                    context.SaveChanges();
                    HttpContext.Current.Cache.Remove("Student" + "-Category-" + course.subject);
                    HttpContext.Current.Cache.Remove("Teacher" + "-Category-" + course.subject);
                    HttpContext.Current.Cache.Remove("Admin" + "-Category-" + course.subject);
                    HttpContext.Current.Cache.Remove("Anonymous" + "-Category-" + course.subject);
                    HttpContext.Current.Cache.Remove("Student" + "-Category-" + "getAll");
                    HttpContext.Current.Cache.Remove("Teacher" + "-Category-" + "getAll");
                    HttpContext.Current.Cache.Remove("Admin" + "-Category-" + "getAll");
                    HttpContext.Current.Cache.Remove("Anonymous" + "-Category-" + "getAll");
                    HttpContext.Current.Cache.Remove("Item-" + id);
                    return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "Course Deleted" });
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, new { success = false, message = "Course Not Deleted. Not authorized." });
        }
    }
}