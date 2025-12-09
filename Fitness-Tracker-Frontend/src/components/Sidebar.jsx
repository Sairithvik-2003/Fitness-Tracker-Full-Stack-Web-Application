// import { useState } from 'react';
// import { useNavigate, useLocation } from 'react-router-dom';
// import { motion } from 'framer-motion';
// import { 
//   LayoutDashboard, 
//   Target, 
//   Dumbbell, 
//   BarChart2, 
//   Settings, 
//   LogOut 
// } from 'lucide-react';

// function Sidebar() {
//   const [isExpanded, setIsExpanded] = useState(false);
//   const navigate = useNavigate();
//   const location = useLocation();

//   const menuItems = [
//     { path: '/dashboard', icon: LayoutDashboard, label: 'Dashboard' },
//     { path: '/goals', icon: Target, label: 'Fitness Goals' },
//     { path: '/workout', icon: Dumbbell, label: 'Workout' },
//     { path: '/statistics', icon: BarChart2, label: 'Statistics' },
//     { path: '/settings', icon: Settings, label: 'Settings' },
//   ];

//   const handleLogout = () => {
//     localStorage.removeItem('user');
//     navigate('/');
//   };

//   return (
//     <motion.div
//       className="fixed left-0 top-0 h-screen bg-gray-900 text-white"
//       onMouseEnter={() => setIsExpanded(true)}
//       onMouseLeave={() => setIsExpanded(false)}
//       animate={{
//         width: isExpanded ? '240px' : '70px',
//       }}
//       transition={{ duration: 0.3 }}
//     >
//       <div className="flex flex-col h-full">
//         <div className="p-4 flex items-center justify-center">
//           <Dumbbell size={32} className="text-blue-500" />
//         </div>

//         <nav className="flex-1">
//           {menuItems.map((item) => (
//             <motion.div
//               key={item.path}
//               className={`flex items-center px-4 py-3 cursor-pointer ${
//                 location.pathname === item.path ? 'bg-blue-600' : 'hover:bg-gray-800'
//               }`}
//               onClick={() => navigate(item.path)}
//               whileHover={{ x: 5 }}
//             >
//               <item.icon size={24} />
//               <motion.span
//                 className="ml-4"
//                 initial={{ opacity: 0 }}
//                 animate={{ opacity: isExpanded ? 1 : 0 }}
//                 transition={{ duration: 0.2 }}
//               >
//                 {item.label}
//               </motion.span>
//             </motion.div>
//           ))}
//         </nav>

//         <motion.div
//           className="p-4 cursor-pointer hover:bg-gray-800 flex items-center"
//           onClick={handleLogout}
//           whileHover={{ x: 5 }}
//         >
//           <LogOut size={24} />
//           <motion.span
//             className="ml-4"
//             initial={{ opacity: 0 }}
//             animate={{ opacity: isExpanded ? 1 : 0 }}
//             transition={{ duration: 0.2 }}
//           >
//             Logout
//           </motion.span>
//         </motion.div>
//       </div>
//     </motion.div>
//   );
// }

// export default Sidebar;

import { useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { motion } from 'framer-motion';
import { 
  LayoutDashboard, 
  Target, 
  Dumbbell, 
  BarChart2, 
  Settings, 
  LogOut 
} from 'lucide-react';

function Sidebar() {
  const [isExpanded, setIsExpanded] = useState(false);
  const navigate = useNavigate();
  const location = useLocation();

  const menuItems = [
    { path: '/dashboard', icon: LayoutDashboard, label: 'Dashboard' },
    { path: '/goals', icon: Target, label: 'Fitness Goals' },
    { path: '/workout', icon: Dumbbell, label: 'Workout' },
    { path: '/statistics', icon: BarChart2, label: 'Statistics' },
    { path: '/settings', icon: Settings, label: 'Settings' },
  ];

  const handleLogout = () => {
    localStorage.removeItem('user');
    navigate('/');
  };

  return (
    <motion.div
      className="fixed left-0 top-0 h-screen bg-gray-900 text-white"
      onMouseEnter={() => setIsExpanded(true)}
      onMouseLeave={() => setIsExpanded(false)}
      animate={{
        width: isExpanded ? '240px' : '70px',
      }}
      transition={{ duration: 0.3 }}
    >
      <div className="flex flex-col h-full">
        <div className="p-4 flex items-center justify-center">
          <Dumbbell size={32} className="text-blue-500" />
        </div>

        <nav className="flex-1">
          {menuItems.map((item) => (
            <motion.div
              key={item.path}
              className={`flex items-center px-4 py-3 cursor-pointer ${
                location.pathname === item.path ? 'bg-blue-600' : 'hover:bg-gray-800'
              }`}
              onClick={() => navigate(item.path)}
              whileHover={{ x: 5 }}
            >
              <item.icon size={24} />
              <motion.span
                className="ml-4"
                initial={{ opacity: 0 }}
                animate={{ opacity: isExpanded ? 1 : 0 }}
                transition={{ duration: 0.2 }}
              >
                {item.label}
              </motion.span>
            </motion.div>
          ))}
        </nav>

        <motion.div
          className="p-4 cursor-pointer hover:bg-gray-800 flex items-center"
          onClick={handleLogout}
          whileHover={{ x: 5 }}
        >
          <LogOut size={24} />
          <motion.span
            className="ml-4"
            initial={{ opacity: 0 }}
            animate={{ opacity: isExpanded ? 1 : 0 }}
            transition={{ duration: 0.2 }}
          >
            Logout
          </motion.span>
        </motion.div>
      </div>
    </motion.div>
  );
}

export default Sidebar;
