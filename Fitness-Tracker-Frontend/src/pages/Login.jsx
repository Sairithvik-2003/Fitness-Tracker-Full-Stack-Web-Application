

import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { Formik, Form, Field } from 'formik';
import * as Yup from 'yup';
import { motion } from 'framer-motion';
import { Dumbbell } from 'lucide-react';
import axios from 'axios';

const loginSchema = Yup.object().shape({
  email: Yup.string().email('Invalid email').required('Required'),
  password: Yup.string().required('Required'),
});

function Login() {
  const navigate = useNavigate();
  const [error, setError] = useState('');

  const handleSubmit = async (values) => {
    try {
      const response = await axios.get('http://localhost:5007/api/User/GetUserDetailsByAuthentication', {
        params: {
          Email: values.email,
          password: values.password
        }
      });

      if (response.data) {
        localStorage.setItem('user', JSON.stringify(response.data));
        navigate('/dashboard');
        alert("Login Successful!");
      }
    } catch (err) {
      setError('Invalid credentials');
    }
  };

  return (
    <div className="min-h-screen flex">
      <motion.div 
        initial={{ x: -100, opacity: 0 }}
        animate={{ x: 0, opacity: 1 }}
        transition={{ duration: 0.5 }}
        className="hidden lg:flex lg:w-1/2 bg-cover bg-center items-center justify-center p-12"
        style={{
          backgroundImage: 'linear-gradient(rgba(0, 0, 0, 0.5), rgba(0, 0, 0, 0.5)), url("https://images.unsplash.com/photo-1534438327276-14e5300c3a48?ixlib=rb-1.2.1&auto=format&fit=crop&w=1950&q=80")',
          backgroundBlendMode: 'overlay'
        }}
        
      />
      
      
      <motion.div 
        initial={{ x: 100, opacity: 0 }}
        animate={{ x: 0, opacity: 1 }}
        transition={{ duration: 0.5 }}
        className="w-1/2 flex items-center justify-center bg-white"
      >
        <div className="max-w-md w-full px-6">
          <div className="text-center mb-8">
            <Dumbbell className="w-12 h-12 mx-auto text-blue-600" />
            <h2 className="mt-6 text-3xl font-bold text-gray-900">Welcome to FitTrack</h2>
            {/* <p className="mt-2 text-sm text-gray-600">Please sign in to your account</p> */}
          </div>

          <Formik
            initialValues={{ email: '', password: '' }}
            validationSchema={loginSchema}
            onSubmit={handleSubmit}
          >
            {({ errors, touched }) => (
              <Form className="space-y-6">
                <div>
                  <label htmlFor="email" className="block text-sm font-medium text-gray-700">
                    Email
                  </label>
                  <Field
                    id="email"
                    name="email"
                    type="email"
                    className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500"
                  />
                  {errors.email && touched.email && (
                    <div className="text-red-500 text-sm mt-1">{errors.email}</div>
                  )}
                </div>

                <div>
                  <label htmlFor="password" className="block text-sm font-medium text-gray-700">
                    Password
                  </label>
                  <Field
                    id="password"
                    name="password"
                    type="password"
                    className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500"
                  />
                  {errors.password && touched.password && (
                    <div className="text-red-500 text-sm mt-1">{errors.password}</div>
                  )}
                </div>

                {error && (
                  <div className="text-red-500 text-sm text-center">{error}</div>
                )}

                <button
                  type="submit"
                  className="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
                >
                  Sign in
                </button>

                <p className="text-center text-sm text-gray-600">
                  Don't have an account?{' '}
                  <Link to="/register" className="font-medium text-blue-600 hover:text-blue-500">
                    Sign up
                  </Link>
                </p>
              </Form>
            )}
          </Formik>
        </div>
      </motion.div>
    </div>
  );
}

export default Login;




