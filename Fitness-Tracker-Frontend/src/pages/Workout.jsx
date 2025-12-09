import { useState, useEffect } from 'react';
import { motion } from 'framer-motion';
import Layout from '../components/Layout';
import axios from 'axios';
import { Dumbbell, Plus, Trash, Edit, Check } from 'lucide-react';
import { useNavigate } from 'react-router-dom';

function Workout() {
    const [activeTab, setActiveTab] = useState('workoutDetails');
    const [workouts, setWorkouts] = useState([]);
    const [goals, setGoals] = useState([]);
    const [userId, setUserId] = useState('');
    const [error, setError] = useState('');
    const [currentWorkout, setCurrentWorkout] = useState({});
    const user = JSON.parse(localStorage.getItem('user'));
    const [workoutForm, setWorkoutForm] = useState({
        WorkoutType: '',
        WorkoutName: '',
        Duration: '',
        distance: '',
        caloriesBurned: '',
        Date: new Date().toISOString().split('T')[0],
        UserId: user.userId,
    });

    const navigate = useNavigate(); // For navigation

    useEffect(() => {
        fetchWorkouts();
    }, []);

    useEffect(() => {
        if (activeTab === 'addWorkout') {
            fetchGoals();
        }
    }, [activeTab]);

    const fetchWorkouts = async () => {
        try {
            const response = await axios.get(`http://localhost:5007/api/Workout/GetWorkOutDetailsByUserID/UserHistoryOfActivities?userId=${user.userId}`);
            setWorkouts(response.data);
        } catch (error) {
            setError('Failed to fetch workouts');
        }
    };

    const fetchGoals = async () => {
        try {
            const response = await axios.get('http://localhost:5007/api/FitnessGoal/GetGoals');
            setGoals(response.data);
        } catch (error) {
            setError('Failed to fetch goals');
        }
    };

    const handleAddWorkout = async (e) => {
        e.preventDefault();
        const today = new Date().toISOString().split('T')[0];
        if (workoutForm.Date > today) {
            setError('Date cannot be in the future.');
            return;
        } else if (workoutForm.Date < today) {
            setError('Date cannot be in the past.');
            return;
        }
        try {
            await axios.post('http://localhost:5007/api/Workout/AddNewActivityOfUser', null,
                {
                    params: {
                        ...workoutForm
                    }
                }
            );
            fetchWorkouts();
            setActiveTab('workoutDetails');
        } catch (error) {
            setError('Failed to add workout');
        }
    };

    const handleDeleteWorkout = async (workoutId) => {
        try {
            await axios.delete(`http://localhost:5007/api/Workout/DeleteAndDisplayAll?activityId=${workoutId}`)
            fetchWorkouts();
        } catch (error) {
            setError('Failed to delete workout');
        }
    };

    const handleUpdateWorkout = async (e) => {
        e.preventDefault();
        const today = new Date().toISOString().split('T')[0];
        if (workoutForm.Date > today) {
            setError('Date cannot be in the future.');
            return;
        } else if (workoutForm.Date < today) {
            setError('Warning: It is previous date data and you cannot change it.');
            return;
        }
        try {
            await axios.put('http://localhost:5007/api/Workout/UpdateWorkoutByDetails', null,
                {
                    params: {
                        ...workoutForm
                    }
                }
            );
            fetchWorkouts();
            setActiveTab('workoutDetails');
        } catch (error) {
            setError('Failed to update workout');
        }
    };

    const handleEditWorkout = (workout) => {
        // Set the workout details in the state and navigate to the update tab
        setWorkoutForm({
            WorkoutType: workout.workoutType,
            WorkoutName: workout.workoutName,
            Duration: workout.duration,
            distance: workout.distance,
            caloriesBurned: workout.caloriesBurned,
            Date: workout.date,
            UserId: workout.userId,
            workoutId: workout.workoutId, // Add workoutId to the form
        });
        setActiveTab('updateWorkout');
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
                        <Dumbbell className="w-8 h-8" />
                        Workout Management
                    </h1>
                    <p className="text-gray-600">Track and manage your workout activities</p>
                </div>

                <div className="bg-white rounded-lg shadow-md overflow-hidden">
                    <div className="border-b border-gray-200">
                        <nav className="flex">
                            {['workoutDetails', 'addWorkout', 'deleteWorkout', 'updateWorkout', 'goalStatus'].map((tab) => (
                                <button
                                    key={tab}
                                    className={`px-6 py-4 text-sm font-medium ${activeTab === tab
                                        ? 'text-blue-600 border-b-2 border-blue-600'
                                        : 'text-gray-500 hover:text-gray-700'
                                        }`}
                                    onClick={() => setActiveTab(tab)}
                                >
                                    {tab.charAt(0).toUpperCase() + tab.slice(1).replace(/([A-Z])/g, ' $1')}
                                </button>
                            ))}
                        </nav>
                    </div>

                    <div className="p-6">
                        {error && (
                            <div className="mb-4 text-red-500 text-sm">{error}</div>
                        )}

                        {activeTab === 'workoutDetails' && (
                            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                                {workouts.length !== 0 ? workouts.map((workout) => (
                                    <motion.div
                                        key={workout.workoutId}
                                        className="bg-gray-50 p-6 rounded-lg relative"
                                        whileHover={{ scale: 1.02 }}
                                    >
                                        <div className="absolute top-2 right-2">
                                            <button
                                                onClick={() => handleEditWorkout(workout)}
                                                className="text-blue-600 hover:text-blue-700"
                                            >
                                                <Edit className="w-5 h-5" />
                                            </button>
                                        </div>
                                        <h3 className="text-lg font-semibold mb-2">{workout.workoutName}</h3>
                                        <p className="text-gray-600 mb-2">Type: {workout.workoutType}</p>
                                        <p className="text-gray-600 mb-2">Duration: {workout.duration}</p>
                                        <p className="text-gray-600 mb-2">Distance: {workout.distance}km</p>
                                        <p className="text-gray-600 mb-2">Calories: {workout.caloriesBurned}</p>
                                        <p className="text-gray-600">Date: {workout.date}</p>
                                    </motion.div>
                                )) : <p className='text-red-500'>Workout details not found!</p>}
                            </div>
                        )}

                        {(activeTab === 'addWorkout' || activeTab === 'updateWorkout') && (
                            <form onSubmit={activeTab === 'addWorkout' ? handleAddWorkout : handleUpdateWorkout} className="max-w-md mx-auto space-y-4">
                                <div>
                                    <label className="block text-sm font-medium text-gray-700">Workout Type</label>
                                    <select
                                        value={workoutForm.WorkoutType}
                                        onChange={(e) => setWorkoutForm({ ...workoutForm, WorkoutType: e.target.value })}
                                        className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"
                                        required
                                    >
                                        <option value="" selected disabled>Select Type</option>
                                        <option value="Indoor">Indoor</option>
                                        <option value="Outdoor">Outdoor</option>
                                    </select>
                                </div>

                                <div>
                                    <label className="block text-sm font-medium text-gray-700">Workout Name</label>
                                    <select
                                        value={workoutForm.WorkoutName}
                                        onChange={(e) => setWorkoutForm({ ...workoutForm, WorkoutName: e.target.value })}
                                        className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"
                                        required
                                    >
                                        <option value="">Select Name</option>
                                        {goals.filter(goal => goal.goalType === workoutForm.WorkoutType).map((goal) => (
                                            <option key={goal.fitnessGoalId} value={goal.goalName}>
                                                {goal.goalName}
                                            </option>
                                        ))}
                                    </select>
                                </div>

                                <div>
                                    <label className="block text-sm font-medium text-gray-700">Duration (HH:mm)</label>
                                    <input
                                        type="time"
                                        value={workoutForm.Duration}
                                        onChange={(e) => setWorkoutForm({ ...workoutForm, Duration: e.target.value })}
                                        className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"
                                        required
                                    />
                                </div>

                                <div>
                                    <label className="block text-sm font-medium text-gray-700">Distance (km)</label>
                                    <input
                                        type="number"
                                        step="0.01"
                                        value={workoutForm.distance}
                                        onChange={(e) => setWorkoutForm({ ...workoutForm, distance: e.target.value })}
                                        className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"
                                        required
                                    />
                                </div>

                                <div>
                                    <label className="block text-sm font-medium text-gray-700">Calories Burned</label>
                                    <input
                                        type="number"
                                        value={workoutForm.caloriesBurned}
                                        onChange={(e) => setWorkoutForm({ ...workoutForm, caloriesBurned: e.target.value })}
                                        className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"
                                        required
                                    />
                                </div>

                                <div>
                                    <label className="block text-sm font-medium text-gray-700">Date</label>
                                    <input
                                        type="date"
                                        value={workoutForm.Date}
                                        onChange={(e) => {
                                            const today = new Date().toISOString().split('T')[0];
                                            if (e.target.value > today) {
                                                setError('Date cannot be in the future.');
                                            } else if (e.target.value < today) {
                                                setError('Date cannot be in the past.');
                                            } else {
                                                setError('');
                                                setWorkoutForm({ ...workoutForm, Date: e.target.value });
                                            }
                                        }}
                                        className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"
                                        required
                                    />
                                </div>

                                <button
                                    type="submit"
                                    className="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700"
                                >
                                    {activeTab === 'addWorkout' ? 'Add Workout' : 'Update Workout'}
                                </button>
                            </form>
                        )}

                        {activeTab === 'deleteWorkout' && (
                            <div className="space-y-4">
                                {workouts.length !== 0 ? workouts.map((workout) => (
                                    <motion.div
                                        key={workout.workoutId}
                                        className="bg-gray-50 p-4 rounded-lg flex justify-between items-center"
                                        whileHover={{ scale: 1.01 }}
                                    >
                                        <div>
                                            <h3 className="font-semibold">{workout.workoutName}</h3>
                                            <p className="text-sm text-gray-600">Date: {workout.date}</p>
                                        </div>
                                        <button
                                            onClick={() => handleDeleteWorkout(workout.workoutId)}
                                            className="text-red-600 hover:text-red-700"
                                        >
                                            <Trash className="w-5 h-5" />
                                        </button>
                                    </motion.div>
                                )) : <p className='text-red-500'>Workout details not found!</p>}
                            </div>
                        )}

                        {activeTab === 'goalStatus' && (
                            <div className="max-w-md mx-auto space-y-4">
                                <div>
                                    <label className="block text-sm font-medium text-gray-700">Goal Name</label>
                                    <select
                                        className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"
                                        value={workoutForm.WorkoutName}
                                        onChange={(e) => setWorkoutForm({ ...workoutForm, WorkoutName: e.target.value })}
                                    >
                                        <option value="">Select Goal</option>
                                        {goals.map((goal) => (
                                            <option key={goal.fitnessGoalId} value={goal.goalName}>
                                                {goal.goalName}
                                            </option>
                                        ))}
                                    </select>
                                </div>

                                <div>
                                    <label className="block text-sm font-medium text-gray-700">Activity ID</label>
                                    <select
                                        className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"
                                        value={workoutForm.workoutId}
                                        onChange={(e) => setWorkoutForm({ ...workoutForm, workoutId: e.target.value })}
                                    >
                                        <option value="">Select Activity</option>
                                        {workouts.map((workout) => (
                                            <option key={workout.workoutId} value={workout.workoutId}>
                                                {workout.workoutName} - {workout.date}
                                            </option>
                                        ))}
                                    </select>
                                </div>

                                <button
                                    className="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700"
                                    onClick={async () => {
                                        try {
                                            const response = await axios.get(`http://localhost:5007/api/Workout/GoalReachedOrNot?Goalname=${workoutForm.WorkoutName}&activityId=${workoutForm.workoutId}`);
                                            if (response.data.activityName != null) {
                                                if (response.data.isAchieved) {
                                                    setError('Congratulations! Goal achieved! 🎉');
                                                } else {
                                                    setError('Keep going! Goal not yet reached.');
                                                }
                                            } else {
                                                setError('Activity Names are not Matched');
                                            }
                                        } catch (error) {
                                            setError('Failed to check goal status');
                                        }
                                    }}
                                >
                                    Check Goal Status
                                </button>
                            </div>
                        )}
                    </div>
                </div>
            </motion.div>
        </Layout>
    );
}

export default Workout;


// --------------------------------------------------------------------------------------------------


//ORIGINAL CODE THAT WORKS

// import { useState, useEffect } from 'react';
// import { motion } from 'framer-motion';
// import Layout from '../components/Layout';
// import axios from 'axios';
// import { Dumbbell, Plus, Trash, Edit, Check } from 'lucide-react';
// import { useNavigate } from 'react-router-dom';

// function Workout() {
//     const [activeTab, setActiveTab] = useState('workoutDetails');
//     const [workouts, setWorkouts] = useState([]);
//     const [goals, setGoals] = useState([]);
//     const [userId, setUserId] = useState('');
//     const [error, setError] = useState('');
//     const [currentWorkout, setCurrentWorkout] = useState({});
//     const user = JSON.parse(localStorage.getItem('user'));
//     const [workoutForm, setWorkoutForm] = useState({
//         WorkoutType: '',
//         WorkoutName: '',
//         Duration: '',
//         distance: '',
//         caloriesBurned: '',
//         Date: new Date().toISOString().split('T')[0],
//         UserId: user.userId,
//     });

//     const navigate = useNavigate(); // For navigation

//     // console.log(workoutForm);

//     useEffect(() => {
//         fetchWorkouts();
//     }, []);

//     useEffect(() => {
//         if (activeTab === 'addWorkout') {
//             fetchGoals();
//         }
//     }, [activeTab]);

//     const fetchWorkouts = async () => {
//         try {
//             const response = await axios.get(`http://localhost:5007/api/Workout/GetWorkOutDetailsByUserID/UserHistoryOfActivities?userId=${user.userId}`);
//             // console.log(response);
//             setWorkouts(response.data);
//         } catch (error) {
//             setError('Failed to fetch workouts');
//         }
//     };

//     const fetchGoals = async () => {
//         try {
//             console.log("Fetching fitness goals...");
//             const response = await axios.get('http://localhost:5007/api/FitnessGoal/GetGoals');
//             console.log("Fetched goals:", response.data);
//             setGoals(response.data);
//         } catch (error) {
//             console.error("Error fetching goals:", error);
//             setError('Failed to fetch goals');
//         }
//     };

//     const handleAddWorkout = async (e) => {
//         e.preventDefault();
//         try {
//             await axios.post('http://localhost:5007/api/Workout/AddNewActivityOfUser', null,
//                 {
//                     params: {
//                         ...workoutForm
//                     }
//                 }
//             );
//             fetchWorkouts();
//             setActiveTab('workoutDetails');
//         } catch (error) {
//             setError('Failed to add workout');
//         }
//     };

//     const handleDeleteWorkout = async (workoutId) => {
//         try {
//             await axios.delete(`http://localhost:5007/api/Workout/DeleteAndDisplayAll?activityId=${workoutId}`)
//             fetchWorkouts();
//         } catch (error) {
//             setError('Failed to delete workout');
//         }
//     };

//     const handleUpdateWorkout = async (e) => {
//         e.preventDefault();
//         try {
//             await axios.put('http://localhost:5007/api/Workout/UpdateWorkoutByDetails', null,
//                 {
//                     params: {
//                         ...workoutForm
//                     }
//                 }
//             );
//             fetchWorkouts();
//             setActiveTab('workoutDetails');
//         } catch (error) {
//             setError('Failed to update workout');
//         }
//     };

//     const handleEditWorkout = (workout) => {
//         // Set the workout details in the state and navigate to the update tab
//         setWorkoutForm({
//             WorkoutType: workout.workoutType,
//             WorkoutName: workout.workoutName,
//             Duration: workout.duration,
//             distance: workout.distance,
//             caloriesBurned: workout.caloriesBurned,
//             Date: workout.date,
//             UserId: workout.userId,
//             workoutId: workout.workoutId, // Add workoutId to the form
//         });
//         setActiveTab('updateWorkout');
//     };

//     return (
//         <Layout>
//             <motion.div
//                 initial={{ opacity: 0, y: 20 }}
//                 animate={{ opacity: 1, y: 0 }}
//                 transition={{ duration: 0.5 }}
//             >
//                 <div className="mb-8">
//                     <h1 className="text-3xl font-bold text-gray-900 flex items-center gap-2">
//                         <Dumbbell className="w-8 h-8" />
//                         Workout Management
//                     </h1>
//                     <p className="text-gray-600">Track and manage your workout activities</p>
//                 </div>

//                 <div className="bg-white rounded-lg shadow-md overflow-hidden">
//                     <div className="border-b border-gray-200">
//                         <nav className="flex">
//                             {['workoutDetails', 'addWorkout', 'deleteWorkout', 'updateWorkout', 'goalStatus'].map((tab) => (
//                                 <button
//                                     key={tab}
//                                     className={`px-6 py-4 text-sm font-medium ${activeTab === tab
//                                         ? 'text-blue-600 border-b-2 border-blue-600'
//                                         : 'text-gray-500 hover:text-gray-700'
//                                         }`}
//                                     onClick={() => setActiveTab(tab)}
//                                 >
//                                     {tab.charAt(0).toUpperCase() + tab.slice(1).replace(/([A-Z])/g, ' $1')}
//                                 </button>
//                             ))}
//                         </nav>
//                     </div>

//                     <div className="p-6">
//                         {error && (
//                             <div className="mb-4 text-red-500 text-sm">{error}</div>
//                         )}

//                         {activeTab === 'workoutDetails' && (
//                             <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
//                                 {workouts.length !== 0 ? workouts.map((workout) => (
//                                     <motion.div
//                                         key={workout.workoutId}
//                                         className="bg-gray-50 p-6 rounded-lg relative"
//                                         whileHover={{ scale: 1.02 }}
//                                     >
//                                         <div className="absolute top-2 right-2">
//                                             <button
//                                                 onClick={() => handleEditWorkout(workout)}
//                                                 className="text-blue-600 hover:text-blue-700"
//                                             >
//                                                 <Edit className="w-5 h-5" />
//                                             </button>
//                                         </div>
//                                         <h3 className="text-lg font-semibold mb-2">{workout.workoutName}</h3>
//                                         <p className="text-gray-600 mb-2">Type: {workout.workoutType}</p>
//                                         <p className="text-gray-600 mb-2">Duration: {workout.duration}</p>
//                                         <p className="text-gray-600 mb-2">Distance: {workout.distance}km</p>
//                                         <p className="text-gray-600 mb-2">Calories: {workout.caloriesBurned}</p>
//                                         <p className="text-gray-600">Date: {workout.date}</p>
//                                     </motion.div>
//                                 )) : <p className='text-red-500'>Workout details not found!</p>}
//                             </div>
//                         )}

//                         {(activeTab === 'addWorkout' || activeTab === 'updateWorkout') && (
//                             <form onSubmit={activeTab === 'addWorkout' ? handleAddWorkout : handleUpdateWorkout} className="max-w-md mx-auto space-y-4">
//                                 <div>
//                                     <label className="block text-sm font-medium text-gray-700">Workout Type</label>
//                                     <select
//                                         value={workoutForm.WorkoutType}
//                                         onChange={(e) => setWorkoutForm({ ...workoutForm, WorkoutType: e.target.value })}
//                                         className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"
//                                         required
//                                     >
//                                         <option value="" selected disabled>Select Type</option>
//                                         <option value="Indoor">Indoor</option>
//                                         <option value="Outdoor">Outdoor</option>
//                                     </select>
//                                 </div>

//                                 <div>
//                                     <label className="block text-sm font-medium text-gray-700">Workout Name</label>
//                                     <select
//                                         value={workoutForm.WorkoutName}
//                                         onChange={(e) => setWorkoutForm({ ...workoutForm, WorkoutName: e.target.value })}
//                                         className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"
//                                         required
//                                     >
//                                         <option value="">Select Name</option>
//                                         {goals.filter(goal => goal.goalType === workoutForm.WorkoutType).map((goal) => (
//                                             <option key={goal.fitnessGoalId} value={goal.goalName}>
//                                                 {goal.goalName}
//                                             </option>
//                                         ))}
//                                     </select>
//                                 </div>

//                                 <div>
//                                     <label className="block text-sm font-medium text-gray-700">Duration (HH:mm)</label>
//                                     <input
//                                         type="time"
//                                         value={workoutForm.Duration}
//                                         onChange={(e) => setWorkoutForm({ ...workoutForm, Duration: e.target.value })}
//                                         className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"
//                                         required
//                                     />
//                                 </div>

//                                 <div>
//                                     <label className="block text-sm font-medium text-gray-700">Distance (km)</label>
//                                     <input
//                                         type="number"
//                                         step="0.01"
//                                         value={workoutForm.distance}
//                                         onChange={(e) => setWorkoutForm({ ...workoutForm, distance: e.target.value })}
//                                         className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"
//                                         required
//                                     />
//                                 </div>

//                                 <div>
//                                     <label className="block text-sm font-medium text-gray-700">Calories Burned</label>
//                                     <input
//                                         type="number"
//                                         value={workoutForm.caloriesBurned}
//                                         onChange={(e) => setWorkoutForm({ ...workoutForm, caloriesBurned: e.target.value })}
//                                         className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"
//                                         required
//                                     />
//                                 </div>

//                                 <div>
//                                     <label className="block text-sm font-medium text-gray-700">Date</label>
//                                     <input
//                                         type="date"
//                                         value={workoutForm.Date}
//                                         onChange={(e) => setWorkoutForm({ ...workoutForm, Date: e.target.value })}
//                                         className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"
//                                         required
//                                     />
//                                 </div>

//                                 <button
//                                     type="submit"
//                                     className="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700"
//                                 >
//                                     {activeTab === 'addWorkout' ? 'Add Workout' : 'Update Workout'}
//                                 </button>
//                             </form>
//                         )}

//                         {activeTab === 'deleteWorkout' && (
//                             <div className="space-y-4">
//                                 {workouts.length !== 0 ? workouts.map((workout) => (
//                                     <motion.div
//                                         key={workout.workoutId}
//                                         className="bg-gray-50 p-4 rounded-lg flex justify-between items-center"
//                                         whileHover={{ scale: 1.01 }}
//                                     >
//                                         <div>
//                                             <h3 className="font-semibold">{workout.workoutName}</h3>
//                                             <p className="text-sm text-gray-600">Date: {workout.date}</p>
//                                         </div>
//                                         <button
//                                             onClick={() => handleDeleteWorkout(workout.workoutId)}
//                                             className="text-red-600 hover:text-red-700"
//                                         >
//                                             <Trash className="w-5 h-5" />
//                                         </button>
//                                     </motion.div>
//                                 )) : <p className='text-red-500'>Workout details not found!</p>}
//                             </div>
//                         )}

//                         {activeTab === 'goalStatus' && (
//                             <div className="max-w-md mx-auto space-y-4">
//                                 <div>
//                                     <label className="block text-sm font-medium text-gray-700">Goal Name</label>
//                                     <select
//                                         className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"
//                                         value={workoutForm.WorkoutName}
//                                         onChange={(e) => setWorkoutForm({ ...workoutForm, WorkoutName: e.target.value })}
//                                     >
//                                         <option value="">Select Goal</option>
//                                         {goals.map((goal) => (
//                                             <option key={goal.fitnessGoalId} value={goal.goalName}>
//                                                 {goal.goalName}
//                                             </option>
//                                         ))}
//                                     </select>
//                                 </div>

//                                 <div>
//                                     <label className="block text-sm font-medium text-gray-700">Activity ID</label>
//                                     <select
//                                         className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"
//                                         value={workoutForm.workoutId}
//                                         onChange={(e) => setWorkoutForm({ ...workoutForm, workoutId: e.target.value })}
//                                     >
//                                         <option value="">Select Activity</option>
//                                         {workouts.map((workout) => (
//                                             <option key={workout.workoutId} value={workout.workoutId}>
//                                                 {workout.workoutName} - {workout.date}
//                                             </option>
//                                         ))}
//                                     </select>
//                                 </div>

//                                 <button
//                                     className="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700"
//                                     onClick={async () => {
//                                         try {
//                                             console.log(workoutForm);

//                                             // const response=await axios.get(`http://localhost:5007/api/Workout/GoalReachedOrNot?Goalname=${workouts.workoutName}activityId=${workouts.workoutId}`
//                                             // );
//                                             const response = await axios.get(`http://localhost:5007/api/Workout/GoalReachedOrNot?Goalname=${workoutForm.WorkoutName}&activityId=${workoutForm.workoutId}`);
//                                             console.log(response);
//                                             if (response.data.activityName != null) {

//                                                 if (response.data.isAchieved) {
//                                                     setError('Congratulations! Goal achieved! 🎉');
//                                                 } else {
//                                                     setError('Keep going! Goal not yet reached.');
//                                                 }
//                                             }
//                                             else {
//                                                 setError('Activity Names are not Matched');
//                                             }

//                                         } catch (error) {
//                                             setError('Failed to check goal status');
//                                         }
//                                     }}
//                                 >
//                                     Check Goal Status
//                                 </button>
//                             </div>
//                         )}
//                     </div>
//                 </div>
//             </motion.div>
//         </Layout>
//     );
// }

// export default Workout;



