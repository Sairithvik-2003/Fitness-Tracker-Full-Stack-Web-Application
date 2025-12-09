import { useState, useEffect } from 'react';
import { motion } from 'framer-motion';
import Layout from '../components/Layout';
import axios from 'axios';
import { Target } from 'lucide-react';

function FitnessGoals() {
  const [activeTab, setActiveTab] = useState('goalPlan');
  const [goals, setGoals] = useState([]);
  const [selectedGoal, setSelectedGoal] = useState(null);
  const [goalName, setGoalName] = useState('');
  const [error, setError] = useState('');

  useEffect(() => {
    if (activeTab === 'goalPlan') {
      fetchGoals();
    }
  }, [activeTab]);

  const fetchGoals = async () => {
    try {
      const response = await axios.get('http://localhost:5007/api/FitnessGoal/GetGoals');
      setGoals(response.data);
    } catch (error) {
      setError('Failed to fetch goals');
    }
  };

  const handleGoalSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.get('http://localhost:5007/api/FitnessGoal/GetGoalByName', {
        params: { GoalName: goalName }
      });
      setSelectedGoal(response.data);
      setError('');
    } catch (error) {
      setError('Goal not found');
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
          <h1 className="text-3xl font-bold text-gray-900 flex items-center gap-2">
            <Target className="w-8 h-8" />
            Fitness Goals
          </h1>
          <p className="text-gray-600">Set and track your fitness objectives</p>
        </div>

        <div className="bg-white rounded-lg shadow-md overflow-hidden">
          <div className="border-b border-gray-200">
            <nav className="flex">
              <button
                className={`px-6 py-4 text-sm font-medium ${
                  activeTab === 'goalPlan'
                    ? 'text-blue-600 border-b-2 border-blue-600'
                    : 'text-gray-500 hover:text-gray-700'
                }`}
                onClick={() => setActiveTab('goalPlan')}
              >
                Goal Plan
              </button>
              <button
                className={`px-6 py-4 text-sm font-medium ${
                  activeTab === 'goal'
                    ? 'text-blue-600 border-b-2 border-blue-600'
                    : 'text-gray-500 hover:text-gray-700'
                }`}
                onClick={() => setActiveTab('goal')}
              >
                Goal
              </button>
            </nav>
          </div>

          <div className="p-6">
            {activeTab === 'goalPlan' ? (
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                {goals.map((goal) => (
                  <motion.div
                    key={goal.fitnessGoalId}
                    className="bg-gray-50 p-6 rounded-lg"
                    whileHover={{ scale: 1.02 }}
                  >
                    <h3 className="text-lg font-semibold mb-2">{goal.goalName}</h3>
                    <p className="text-gray-600 mb-2">Type: {goal.goalType}</p>
                    <p className="text-gray-600 mb-2">Duration: {goal.duration}</p>
                    <p className="text-gray-600 mb-2">Distance: {goal.distance}km</p>
                    <p className="text-gray-600">Calories: {goal.caloriesBurned}</p>
                  </motion.div>
                ))}
              </div>
            ) : (
              <div className="max-w-md mx-auto">
                <form onSubmit={handleGoalSubmit} className="space-y-4">
                  <div>
                    <label className="block text-sm font-medium text-gray-700">
                      Goal Name
                    </label>
                    <input
                      type="text"
                      value={goalName}
                      onChange={(e) => setGoalName(e.target.value)}
                      className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500"
                      required
                    />
                  </div>

                  {error && (
                    <p className="text-red-500 text-sm">{error}</p>
                  )}

                  <button
                    type="submit"
                    className="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700"
                  >
                    Search Goal
                  </button>
                </form>

                {selectedGoal && (
                  <motion.div
                    initial={{ opacity: 0, y: 20 }}
                    animate={{ opacity: 1, y: 0 }}
                    className="mt-6 bg-gray-50 p-6 rounded-lg"
                  >
                    <h3 className="text-lg font-semibold mb-2">{selectedGoal.goalName}</h3>
                    <p className="text-gray-600 mb-2">Type: {selectedGoal.goalType}</p>
                    <p className="text-gray-600 mb-2">Duration: {selectedGoal.duration}</p>
                    <p className="text-gray-600 mb-2">Distance: {selectedGoal.distance}km</p>
                    <p className="text-gray-600">Calories: {selectedGoal.caloriesBurned}</p>
                  </motion.div>
                )}
              </div>
            )}
          </div>
        </div>
      </motion.div>
    </Layout>
  );
}

export default FitnessGoals;