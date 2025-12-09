import { useState, useEffect } from 'react';
import { motion } from 'framer-motion';
import Layout from '../components/Layout';
import axios from 'axios';
import { User, Trash2 } from 'lucide-react';
import { useNavigate } from 'react-router-dom';

function Settings() {
  const [activeTab, setActiveTab] = useState('profile');
  const [userData, setUserData] = useState(null);
  const [error, setError] = useState('');
  const [showDeleteConfirm, setShowDeleteConfirm] = useState(false);
  const navigate = useNavigate();
  const user = JSON.parse(localStorage.getItem('user'));

  useEffect(() => {
    fetchUserProfile();
  }, []);

  const fetchUserProfile = async () => {
    try {
      const response = await axios.get('http://localhost:5007/api/User/GetUserProfileByID', {
        params: { id: user.userId }
      });
      setUserData(response.data);
    } catch (error) {
      setError('Failed to fetch user profile');
    }
  };

  const handleDeleteAccount = async () => {
    try {
      const response = await axios.delete('http://localhost:5007/api/User/DeleteUser', {
        params: { UserId: user.userId }
      });
      
      if (response.data) {
        localStorage.removeItem('user');
        navigate('/');
      } else {
        setError('Failed to delete account');
      }
    } catch (error) {
      setError('Error deleting account');
    }
  };

  return (
    <Layout>
      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.5 }}
      >
        <div className="mb-8">
          <h1 className="text-3xl font-bold text-gray-900">Settings</h1>
          <p className="text-gray-600">Manage your account preferences</p>
        </div>

        <div className="bg-white rounded-lg shadow-md overflow-hidden">
          <div className="border-b border-gray-200">
            <nav className="flex">
              <button
                className={`px-6 py-4 text-sm font-medium ${
                  activeTab === 'profile'
                    ? 'text-blue-600 border-b-2 border-blue-600'
                    : 'text-gray-500 hover:text-gray-700'
                }`}
                onClick={() => setActiveTab('profile')}
              >
                Profile
              </button>
              <button
                className={`px-6 py-4 text-sm font-medium ${
                  activeTab === 'deleteAccount'
                    ? 'text-blue-600 border-b-2 border-blue-600'
                    : 'text-gray-500 hover:text-gray-700'
                }`}
                onClick={() => setActiveTab('deleteAccount')}
              >
                Delete Account
              </button>
            </nav>
          </div>

          <div className="p-6">
            {error && (
              <div className="mb-4 text-red-500 text-sm">{error}</div>
            )}

            {activeTab === 'profile' && userData && (
              <motion.div
                initial={{ opacity: 0 }}
                animate={{ opacity: 1 }}
                className="max-w-2xl"
              >
                <div className="flex items-center gap-4 mb-6">
                  <div className="p-3 bg-blue-100 rounded-full">
                    <User className="w-6 h-6 text-blue-600" />
                  </div>
                  <div>
                    <h2 className="text-xl font-semibold">{userData.fullName}</h2>
                    <p className="text-gray-600">{userData.email}</p>
                  </div>
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                  <div>
                    <h3 className="text-sm font-medium text-gray-500">Personal Information</h3>
                    <div className="mt-2 space-y-4">
                      <div>
                        <label className="block text-sm text-gray-500">Gender</label>
                        <p className="text-gray-900">{userData.gender}</p>
                      </div>
                      <div>
                        <label className="block text-sm text-gray-500">Date of Birth</label>
                        <p className="text-gray-900">{userData.dateOfBirth}</p>
                      </div>
                    </div>
                  </div>

                  <div>
                    <h3 className="text-sm font-medium text-gray-500">Physical Information</h3>
                    {/* <div className="mt-2 <boltAction type="file" filePath="src/pages/Settings.jsx">                     */}
                    <div className="mt-2 space-y-4">
                      <div>
                        <label className="block text-sm text-gray-500">Height</label>
                        <p className="text-gray-900">{userData.height} cm</p>
                      </div>
                      <div>
                        <label className="block text-sm text-gray-500">Weight</label>
                        <p className="text-gray-900">{userData.weight} kg</p>
                      </div>
                    </div>
                  </div>
                </div>
              </motion.div>
            )}

            {activeTab === 'deleteAccount' && (
              <motion.div
                initial={{ opacity: 0 }}
                animate={{ opacity: 1 }}
                className="max-w-md mx-auto text-center"
              >
                {!showDeleteConfirm ? (
                  <div className="space-y-4">
                    <div className="p-3 bg-red-100 rounded-full inline-block">
                      <Trash2 className="w-6 h-6 text-red-600" />
                    </div>
                    <h2 className="text-xl font-semibold">Delete Account</h2>
                    <p className="text-gray-600">
                      Are you sure you want to delete your account? This action cannot be undone.
                    </p>
                    <button
                      onClick={() => setShowDeleteConfirm(true)}
                      className="px-4 py-2 bg-red-600 text-white rounded-md hover:bg-red-700"
                    >
                      Delete Account
                    </button>
                  </div>
                ) : (
                  <div className="space-y-4">
                    <h2 className="text-xl font-semibold">Confirm Deletion</h2>
                    <p className="text-gray-600">
                      Please confirm that you want to permanently delete your account.
                    </p>
                    <div className="flex justify-center gap-4">
                      <button
                        onClick={() => setShowDeleteConfirm(false)}
                        className="px-4 py-2 bg-gray-200 text-gray-700 rounded-md hover:bg-gray-300"
                      >
                        Cancel
                      </button>
                      <button
                        onClick={handleDeleteAccount}
                        className="px-4 py-2 bg-red-600 text-white rounded-md hover:bg-red-700"
                      >
                        Yes, Delete My Account
                      </button>
                    </div>
                  </div>
                )}
              </motion.div>
            )}
          </div>
        </div>
      </motion.div>
    </Layout>
  );
}

export default Settings;