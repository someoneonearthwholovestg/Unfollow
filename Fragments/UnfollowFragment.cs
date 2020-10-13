﻿using System;
using System.Threading.Tasks;

using Android.Views;

using AndroidX.AppCompat.App;
using ActionMode = AndroidX.AppCompat.View.ActionMode;

using Madamin.Unfollow.Instagram;
using Madamin.Unfollow.Adapters;
using Madamin.Unfollow.ViewHolders;

namespace Madamin.Unfollow.Fragments
{
    public class UnfollowFragment :
        RecyclerViewFragmentBase,
        IUnfollowerItemClickListener,
        ActionMode.ICallback
    {
        public UnfollowFragment(Account account) :
            base()
        {
            _account = account;
            Create += UnfollowFragment_Create;
        }

        private void UnfollowFragment_Create(object sender, OnCreateEventArgs e)
        {
            Title = _account.User.Fullname;
            // TODO: set ErrorText
            // TODO: set EmptyText
            SetEmptyImage(Resource.Drawable.ic_person_remove_black_48dp);

            _adapter = new UnfollowerAdapter(_account, this);
            Adapter = _adapter;
            _adapter.Refresh();
            ViewMode = RecyclerViewMode.Data;
        }

        public bool OnItemClick(int position)
        {
            if (_action_mode != null)
            {
                _select_or_deselect_item(position);
                return true;
            }
            return false;
        }

        public void OnItemLongClick(int position)
        {
            _select_or_deselect_item(position);
        }

        public void OnItemOpen(int position)
        {
            var user = _adapter.GetItem(position);
            ((IInstagramHost)Activity).OpenInInstagram(user.Username);
        }

        public void OnItemSelect(int position)
        {
            _select_or_deselect_item(position);
        }

        public void OnItemUnfollow(int position)
        {
            //_btn_unfollow.Enabled = false;
            try
            {
                DoTask(
                    _account.UnfollowAsync(_adapter.GetItem(position)),
                    _refresh_adapter_data);
            }
            catch (Exception ex)
            {
                //_btn_unfollow.Enabled = true;
                ((IFragmentHost)Activity).ShowError(ex);
            }
        }

        public void OnItemAddToWhitelist(int position)
        {
            // TODO
        }

        private void _select_or_deselect_item(int pos)
        {
            _adapter.SelectOrDeselectItem(pos);

            if (_adapter.SelectedItems.Count <= 0)
            {
                _action_mode?.Finish();
                return;
            }

            if (_action_mode == null)
            {
                _action_mode = ((AppCompatActivity)Activity).StartSupportActionMode(this);
                _action_mode.Title = _account.User.Fullname;
            }

            _action_mode.Subtitle = string.Format(
                GetString(Resource.String.title_selected),
                _adapter.SelectedItems.Count);
        }

        public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.appbar_unfollow_item_selectall:
                    _adapter.SelectAll();
                    _action_mode.Subtitle = string.Format(
                        GetString(Resource.String.title_selected),
                        _adapter.SelectedItems.Count);
                    return true;
                case Resource.Id.appbar_unfollow_item_unfollow:
                    DoTask(BatchUnfollowAsync(
                        _adapter.GetSelected()),
                        _refresh_adapter_data);
                    mode.Finish();
                    return true;
                default:
                    return false;
            }
        }

        public bool OnCreateActionMode(ActionMode mode, IMenu menu)
        {
            mode.MenuInflater
                .Inflate(Resource.Menu.appbar_menu_unfollow_contextual, menu);
            return true;
        }

        public void OnDestroyActionMode(ActionMode mode)
        {
            _adapter.DeselectAll();
            mode.Dispose();
            _action_mode = null;
        }

        public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
        {
            return false;
        }

        private async Task BatchUnfollowAsync(User[] users)
        {
            for (var i = 0; i < users.Length; i++)
            {
                _update_progress(i, users.Length);
                await _account.UnfollowAsync(users[i]);
            }
            ProgressText = GetString(Resource.String.title_loading);
        }

        private void _update_progress(int i, int total)
        {
            ProgressText = string.Format(
                    GetString(Resource.String.title_batch_unfollow), i, total);
        }

        private void _refresh_adapter_data()
        {
            _adapter.Refresh();
            _adapter.NotifyDataSetChanged();
            ((IInstagramHost)Activity).Accounts.SaveAccountCache(_account);
        }

        private Account _account;
        private UnfollowerAdapter _adapter;
        private ActionMode _action_mode;
    }
}
