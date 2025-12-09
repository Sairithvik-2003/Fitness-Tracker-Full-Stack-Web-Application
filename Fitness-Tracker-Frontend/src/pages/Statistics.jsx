import { useState, useEffect } from 'react';
import { motion } from 'framer-motion';
import Layout from '../components/Layout';
import {
  LineChart,
  Line,
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend,
  ResponsiveContainer
} from 'recharts';
import axios from 'axios';
import { Trophy, Activity, Flame } from 'lucide-react';
 
function Statistics() {
  const [workouts, setWorkouts] = useState([]);
  const [achievedGoals, setAchievedGoals] = useState(0);
  const [dailyCalories, setDailyCalories] = useState([]);
  const [error, setError] = useState('');
  const user = JSON.parse(localStorage.getItem('user'));
 
  useEffect(() => {
    if (user) {
      fetchWorkouts();
    }
  }, [user]);
 
  const fetchWorkouts = async () => {
    try {
const response = await axios.get('http://localhost:5007/api/Workout/GetWorkOutDetailsByUserID/UserHistoryOfActivities', {
        params: { userId: user.userId }
      });
      
      // Format workout data for bar chart
      const formattedWorkouts = response.data.map(workout => ({
        ...workout,
label: `(${new Date(workout.date).toLocaleDateString()})`
      }));
      
      setWorkouts(formattedWorkouts);
      
      // Process daily calories
      const caloriesByDate = response.data.reduce((acc, workout) => {
const date = workout.date;
        acc[date] = (acc[date] || 0) + workout.caloriesBurned;
        return acc;
      }, {});
 
      // Convert to array format for chart
      const dailyCaloriesData = Object.entries(caloriesByDate).map(([date, calories]) => ({
        date,
        calories
})).sort((a, b) => new Date(a.date) - new Date(b.date));
 
      setDailyCalories(dailyCaloriesData);
 
      // Check goals achieved
      const checkGoalsAchieved = async () => {
        let achievedCount = 0;
        for (const workout of response.data) {
          try {
const goalResponse = await axios.get('http://localhost:5007/api/Workout/GoalReachedOrNot', {
              params: {
                Goalname: workout.workoutName,
                activityId: workout.workoutId
              }
            });
            if (goalResponse.data.isAchieved) {
              achievedCount++;
            }
          } catch (error) {
            console.error('Error checking goal:', error);
          }
        }
        setAchievedGoals(achievedCount);
      };
 
      checkGoalsAchieved();
    } catch (error) {
      setError('Failed to fetch workouts');
      console.error('Failed to fetch workouts:', error);
    }
  };
 
  const totalCalories = workouts.reduce((sum, workout) => sum + workout.caloriesBurned, 0);
  const totalDistance = workouts.reduce((sum, workout) => sum + workout.distance, 0);
 
  return (
    <Layout>
      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.5 }}
      >
        <div className="mb-8">
          <h1 className="text-3xl font-bold text-gray-900">Statistics</h1>
          <p className="text-gray-600">Track your progress and achievements</p>
        </div>
 
        <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
          <motion.div
            className="bg-white p-6 rounded-lg shadow-md"
            whileHover={{ scale: 1.02 }}
          >
            <Trophy className="w-8 h-8 text-yellow-500 mb-4" />
            <h3 className="text-lg font-semibold mb-2">Goals Achieved</h3>
            <div className="flex items-end gap-2">
              <p className="text-3xl font-bold text-gray-900">{achievedGoals}</p>
              {Array.from({ length: achievedGoals }).map((_, i) => (
                <Trophy key={i} className="w-6 h-6 text-yellow-500" />
              ))}
            </div>
          </motion.div>
 
          <motion.div
            className="bg-white p-6 rounded-lg shadow-md"
            whileHover={{ scale: 1.02 }}
          >
            <Activity className="w-8 h-8 text-blue-500 mb-4" />
            <h3 className="text-lg font-semibold mb-2">Total Distance</h3>
            <p className="text-3xl font-bold text-gray-900">{totalDistance.toFixed(2)} km</p>
          </motion.div>
 
          <motion.div
            className="bg-white p-6 rounded-lg shadow-md"
            whileHover={{ scale: 1.02 }}
          >
            <Flame className="w-8 h-8 text-red-500 mb-4" />
            <h3 className="text-lg font-semibold mb-2">Total Calories</h3>
            <p className="text-3xl font-bold text-gray-900">{totalCalories}</p>
          </motion.div>
        </div>
 
        {/* Daily Calories Line Chart */}
        <div className="bg-white p-6 rounded-lg shadow-md mb-8">
          <h2 className="text-xl font-semibold mb-4">Daily Calories Burned</h2>
          <div className="h-[400px]">
            <ResponsiveContainer width="100%" height="100%">
              <LineChart data={dailyCalories}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis
                  dataKey="date"
                  tickFormatter={(date) => new Date(date).toLocaleDateString()}
                />
                <YAxis />
                <Tooltip
                  labelFormatter={(date) => new Date(date).toLocaleDateString()}
                  formatter={(value) => [`${value} calories`, 'Calories Burned']}
                />
                <Legend />
                <Line
                  type="monotone"
                  dataKey="calories"
                  stroke="#3B82F6"
                  name="Daily Calories"
                  strokeWidth={2}
                  dot={{ r: 4 }}
                  activeDot={{ r: 8 }}
                />
              </LineChart>
            </ResponsiveContainer>
          </div>
        </div>
 
        {/* Individual Workouts Bar Chart */}
        <div className="bg-white p-6 rounded-lg shadow-md mb-8">
          <h2 className="text-xl font-semibold mb-4">Individual Workout Calories</h2>
          <div className="h-[400px]">
            <ResponsiveContainer width="100%" height="100%">
              <BarChart
                data={workouts}
                margin={{
                  bottom: 100 // Add margin for workout names
                }}
              >
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis
                  dataKey="label"
                  interval={0}
                  angle={-45}
                  textAnchor="end"
                  height={100}
                  tick={{
                    fontSize: 12,
                    fill: '#4B5563'
                  }}
                />
                <YAxis />
                <Tooltip
                  formatter={(value, name, props) => [
                    `${value} calories`,
                    `${props.payload.workoutName}`
                  ]}
                />
                <Legend />
                <Bar
                  dataKey="caloriesBurned"
                  fill="#3B82F6"
                  name="Calories Burned"
                >
                </Bar>
              </BarChart>
            </ResponsiveContainer>
          </div>
        </div>
 
        <div className="bg-white p-6 rounded-lg shadow-md">
          <h2 className="text-xl font-semibold mb-4">Recent Achievements</h2>
          <div className="space-y-4">
            {workouts.slice(0, 5).map((workout, index) => (
              <motion.div
                key={index}
                className="flex items-center gap-4 p-4 bg-gray-50 rounded-lg"
                whileHover={{ scale: 1.01 }}
              >
                <div className="p-2 bg-blue-100 rounded-full">
                  <Activity className="w-6 h-6 text-blue-600" />
                </div>
                <div>
                  <h3 className="font-semibold">{workout.workoutName}</h3>
                  <p className="text-sm text-gray-600">
{new Date(workout.date).toLocaleDateString()} • {workout.caloriesBurned} calories • {workout.distance} km
                  </p>
                </div>
              </motion.div>
            ))}
          </div>
        </div>
 
        {error && (
          <div className="mt-4 p-4 bg-red-100 text-red-700 rounded-lg">
            {error}
          </div>
        )}
      </motion.div>
    </Layout>
  );
}
 
export default Statistics;