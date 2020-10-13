﻿using System;

using Google.Android.Material.Button;
using Google.Android.Material.TextField;

using Madamin.Unfollow.Instagram;

namespace Madamin.Unfollow.Fragments
{
    class LoginFragment : FragmentBase
    {
        public LoginFragment() : base(Resource.Layout.fragment_login) 
        {
            Create += LoginFragment_Create;
        }

        private void LoginFragment_Create(object sender, OnCreateEventArgs e)
        {
            Title = GetString(Resource.String.title_addaccount);

            _et_username = e.View.FindViewById<TextInputEditText>(Resource.Id.fragment_login_et_username);
            _et_password = e.View.FindViewById<TextInputEditText>(Resource.Id.fragment_login_et_password);
            _et_layout_password = e.View.FindViewById<TextInputLayout>(Resource.Id.fragment_login_et_layout_password);
            _btn_login = e.View.FindViewById<MaterialButton>(Resource.Id.fragment_login_btn_login);

            var btn_cancel = e.View.FindViewById<MaterialButton>(Resource.Id.fragment_login_btn_cancel);

            _btn_login.Click += LoginBtn_Click;
            btn_cancel.Click += CancelBtn_Click;
        }

        private async void LoginBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_et_username.Text))
                return; // TODO: Show an error
            if (string.IsNullOrWhiteSpace(_et_password.Text))
                return; // TODO: Show an error

            try
            {
                var ig = ((IInstagramHost)Activity).Accounts;

                _et_username.Enabled = false;
                _et_password.Enabled = false;
                _btn_login.Enabled = false;

                await ig.AddAccountAsync(_et_username.Text, _et_password.Text);

                PopFragment();
            }
            catch (BadPasswordException)
            {
                _et_layout_password.Error = GetString(Resource.String.error_invalid_password);
                _et_password.TextChanged += (et, args) =>
                {
                    _et_layout_password.ErrorEnabled = false;
                };
            }
            catch (Exception ex)
            {
                ((IFragmentHost)Activity).ShowError(ex);
            }
            finally
            {
                _et_username.Enabled = true;
                _et_password.Enabled = true;
                _btn_login.Enabled = true;
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            PopFragment();
        }

        private TextInputEditText _et_username, _et_password;
        private TextInputLayout _et_layout_password;
        private MaterialButton _btn_login;
    }
}