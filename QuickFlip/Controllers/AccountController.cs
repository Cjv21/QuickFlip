using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using QuickFlip.BusinessLayer;
using QuickFlip.Filters;
using QuickFlip.Models;

namespace QuickFlip.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        // GET: /Account/DeletePost/
        [InitializeSimpleMembership]
        public ActionResult DeletePost(int id)
        {
            Post post = BusinessLogic.GetPostByPostId(id);

            if (post.UserId == WebSecurity.CurrentUserId)
            {
                BusinessLogic.DeletePost(post.PostId);
            }
            else
            {
                TempData["UnauthorizedDelete"] = "You cannot delete a post that isn't yours!";
            }

            return RedirectToAction("Manage");
        }

        // GET: /Account/DeletePost/
        [InitializeSimpleMembership]
        public ActionResult EditPost(int id)
        {
            Post post = BusinessLogic.GetPostByPostId(id);

            if (post.UserId == WebSecurity.CurrentUserId)
            {
                if (post.Offers.Count != 0)
                {
                    TempData["UnauthorizedDelete"] = "You cannot edit a post with offers!";
                }
                else
                {
                    // set defaults
                    ViewData["PostType"] = post.PostType == PostType.Sell ? "ForSale" : "WantToBuy";
                    ViewData["AuctionType"] = post.AuctionType == AuctionType.Auction ? "Auction" : "FavoriteOffer";
                    ViewData["LocalOnly"] = post.TransactionType == TransactionType.Local ? "Yes" : "No";

                    var categoryEnums = Enum.GetValues(typeof(Category)).Cast<Category>();
                    foreach (var categoryEnum in categoryEnums)
                    {
                        ViewData[categoryEnum.ToString()] = "0";
                    }

                    foreach (var category in post.Categories)
                    {
                        ViewData[category.ToString()] = "1";
                    }


                    ViewData["PostType"] = "DontCare";
                    ViewData["WillShip"] = "DontCare";
                    ViewData["HasPhoto"] = "DontCare";
                    ViewData["AnyOffers"] = "DontCare";


                    return View(post);
                }
            }
            else
            {
                TempData["UnauthorizedDelete"] = "You cannot edit a post that isn't yours!";
            }

            return RedirectToAction("Manage");
        }

        [HttpPost]
        [InitializeSimpleMembership]
        public ActionResult SubmitEditPost(HttpPostedFileBase postImage)
        {
            int postId = Int32.Parse(Request.Form["PostId"]);

            AuctionType auctionType = (AuctionType)Enum.Parse(
                typeof(AuctionType), Request.Form["AuctionType"].ToString());

            TransactionType transactionType = Request.Form["LocalOnly"].ToString() == "Yes"
                ? TransactionType.Local : TransactionType.LocalOrLongDistance;

            string title = Request.Form["PostTitle"].ToString();

            int communityId = Int32.Parse(Request.Form["CommunityId"]);

            int? maxPrice = (String.IsNullOrWhiteSpace(Request.Form["MaxPrice"]) 
                ? (int?)null : Convert.ToInt32(Request.Form["MaxPrice"]));

            string description = Request.Form["PostDescription"].ToString();

            List<Category> categories = Request.Form["Categories"].ToString().Split(',').Select(
                x => (Category)Enum.Parse(typeof(Category), x)).ToList();

            List<string> tags = new List<string>();
            if (Request.Form["Tags"] != String.Empty)
            {
                tags = Request.Form["Tags"].ToString().Split(',').Select(sValue => sValue.Trim().ToLower()).ToList();
            }

            Post editedPost = new Post()
            {
                PostId = postId,
                CommunityId = communityId,
                UserId = WebSecurity.CurrentUserId,
                CreateDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddMonths(3),
                Title = title,
                Description = description,
                Tags = tags,
                RequiredPrice = maxPrice,
                PostType = PostType.Buy,
                AuctionType = auctionType,
                TransactionType = transactionType,
                Categories = categories
            };

            BusinessLogic.EditPost(editedPost);

            if (postImage != null)
            {
                BusinessLogic.DeletePostMedia(postId);

                byte[] imageBytes = new byte[postImage.InputStream.Length];
                long bytesRead = postImage.InputStream.Read(imageBytes, 0, (int)postImage.InputStream.Length);
                postImage.InputStream.Close();
                string b64EncodedImage = Convert.ToBase64String(imageBytes, 0, imageBytes.Length);

                PostMedia newPostMedia = new PostMedia()
                {
                    PostId = postId,
                    B64EncodedImage = b64EncodedImage
                };
        
                newPostMedia = BusinessLogic.CreatePostMedia(newPostMedia);
            }


            return editedPost.PostType == PostType.Sell
                ? RedirectToAction("Post", "Sell", new { id = postId })
                : RedirectToAction("Post", "Buy", new { id = postId });
        }

        [InitializeSimpleMembership]
        public ActionResult ChangeHomeCommunity()
        {
            int userId = WebSecurity.CurrentUserId;

            CommunityAbbrev comm = (CommunityAbbrev)Enum.Parse(
                typeof(CommunityAbbrev), Request.Form["Community"].ToString()
            );

            BusinessLogic.ChangeHomeCommunity(userId, comm);

            return RedirectToAction("Manage");
        }

        [InitializeSimpleMembership]
        public ActionResult ChangePhone()
        {
            int userId = WebSecurity.CurrentUserId;

            string phoneStr = Request.Form["phone_0-2"] + Request.Form["phone_3-5"] + Request.Form["phone_6-9"];

            Int64 phoneNumber = Int64.Parse(phoneStr);

            BusinessLogic.ChangePhone(userId, phoneNumber);

            return RedirectToAction("Manage");
        }

        [InitializeSimpleMembership]
        public ActionResult ChangeAlertMode()
        {
            int userId = WebSecurity.CurrentUserId;

            AlertMode mode = (AlertMode)Enum.Parse(typeof(AlertMode), Request.Form["AlertMode"].ToString());

            BusinessLogic.ChangeAlertMode(userId, mode);

            return RedirectToAction("Manage");
        }

        // GET: /Account/Manage
        [InitializeSimpleMembership]
        public ActionResult Manage()
        {
            User user = BusinessLogic.GetUserByUserId(WebSecurity.CurrentUserId);
            return View(user);
        }


        // POST: /Account/ChangeProfilePicture
        [HttpPost]
        [InitializeSimpleMembership]
        public ActionResult ChangeProfilePicture(HttpPostedFileBase newProfilePicture)
        {
            // base 64 encode profile picture
            string b64EncodedImage = null;
            if (newProfilePicture != null)
            {
                byte[] imageBytes = new byte[newProfilePicture.InputStream.Length];
                long bytesRead = newProfilePicture.InputStream.Read(imageBytes, 0, (int)newProfilePicture.InputStream.Length);
                newProfilePicture.InputStream.Close();
                b64EncodedImage = Convert.ToBase64String(imageBytes, 0, imageBytes.Length);
            }

            BusinessLogic.ChangeProfilePicture(WebSecurity.CurrentUserId, b64EncodedImage);

            return RedirectToAction("Manage");
        }


        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

       
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            // valid login and verified
            if (ModelState.IsValid && 
                BusinessLogic.IsEmailVerified(model.UserName) && 
                WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }

            // valid login and not verified
            if (ModelState.IsValid &&
                !BusinessLogic.IsEmailVerified(model.UserName) && 
                WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                WebSecurity.Logout();

                // redirect to confirm email page
                var userNameBytes = System.Text.Encoding.UTF8.GetBytes(model.UserName);
                string b64EncodedUserName = Convert.ToBase64String(userNameBytes);

                // store password in clear text, for now...
                TempData["Password"] = model.Password;
                TempData["FirstAttempt"] = true;

                return RedirectToAction("VerifyEmail", "Home", new { id = b64EncodedUserName });
            }

                // if we got this far, something failed, redisplay form
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
                return View(model);
        }


        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

       
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

       
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);

                    string nonce = BusinessLogic.Generate8CharNonce();

                    BusinessLogic.PopulateUserProfile(model, nonce);

                    BusinessLogic.SendConfirmationCode(model, nonce);

                    // redirect to confirm email page
                    var userNameBytes = System.Text.Encoding.UTF8.GetBytes(model.UserName);
                    string b64EncodedUserName = Convert.ToBase64String(userNameBytes);

                    // store password in clear text, for now...
                    TempData["Password"] = model.Password;
                    TempData["FirstAttempt"] = true;

                    return RedirectToAction("AccountCreated", "Home", new { id = b64EncodedUserName }); 
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

       
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ChangePasswordMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ChangePasswordMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("ChangePassword", new { Message = message });
        }

       
        // GET: /Account/ChangePassword
        public ActionResult ChangePassword(ChangePasswordMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ChangePasswordMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ChangePasswordMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ChangePasswordMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("ChangePassword");
            return View();
        }

        
        // POST: /Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("ChangePassword");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("ChangePassword", new { Message = ChangePasswordMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("ChangePassword", new { Message = ChangePasswordMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ChangePasswordMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
