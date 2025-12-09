import { useEffect, useState } from 'react';
import { motion } from 'framer-motion';
import Layout from '../components/Layout';
import { Trophy, Activity, Flame } from 'lucide-react';
import axios from 'axios';

function Dashboard() {
  const [userData, setUserData] = useState(null);
  const user = JSON.parse(localStorage.getItem('user'));
  console.log(user);
  
  useEffect(() => {
    const fetchUserData = async () => {
      try {
        const response = await axios.get('http://localhost:5007/api/User/GetUserProfileByID', {
          params: { id: user.UserId }
        });
        setUserData(response.data);
      } catch (error) {
        console.error('Error fetching user data:', error);
      }
    };

    fetchUserData();
  }, [user.UserId]);

  return (
    <Layout>
      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.5 }}
      >
        <div className="mb-8">
          <h1 className="text-3xl font-bold text-gray-900">
            Welcome back, {user.fullname}!
          </h1>
          <p className="text-gray-600">Track your fitness journey and achieve your goals</p>
        </div>

        {/* <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
          <motion.div
            className="bg-white p-6 rounded-lg shadow-md"
            whileHover={{ scale: 1.02 }}
          >
            <Trophy className="w-8 h-8 text-yellow-500 mb-4" />
            <h3 className="text-lg font-semibold mb-2">Goals Achieved</h3>
            <p className="text-3xl font-bold text-gray-900">3</p>
          </motion.div>

          <motion.div
            className="bg-white p-6 rounded-lg shadow-md"
            whileHover={{ scale: 1.02 }}
          >
            <Activity className="w-8 h-8 text-blue-500 mb-4" />
            <h3 className="text-lg font-semibold mb-2">Active Days</h3>
            <p className="text-3xl font-bold text-gray-900">12</p>
          </motion.div>

          <motion.div
            className="bg-white p-6 rounded-lg shadow-md"
            whileHover={{ scale: 1.02 }}
          >
            <Flame className="w-8 h-8 text-red-500 mb-4" />
            <h3 className="text-lg font-semibold mb-2">Calories Burned</h3>
            <p className="text-3xl font-bold text-gray-900">1,234</p>
          </motion.div>
        </div> */}

        <div className="bg-white rounded-lg shadow-md overflow-hidden">
          <div className="p-6">
            <h2 className="text-xl font-semibold mb-4">Fitness Tracking System</h2>
            <p className="text-gray-600 mb-4">
              Track your workouts, set fitness goals, and monitor your progress all in one place.
              Our comprehensive system helps you stay motivated and achieve your fitness objectives.
            </p>
            <img
              src="https://images.unsplash.com/photo-1517963879433-6ad2b056d712?ixlib=rb-1.2.1&auto=format&fit=crop&w=1950&q=80"
              alt="Fitness"
              className="w-full h-64 object-cover rounded-lg"
            />
          </div>
        </div>
      </motion.div>
    </Layout>
  );
}

export default Dashboard;