
import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { Formik, Form, Field } from 'formik';
import * as Yup from 'yup';
import { motion } from 'framer-motion';
import { Dumbbell } from 'lucide-react';
import axios from 'axios';
import { addYears } from 'date-fns';

const registerSchema = Yup.object().shape({
  name: Yup.string().required('Required'),
  email: Yup.string().email('Invalid email').required('Required'),
  password: Yup.string().min(6, 'Too Short!').required('Required'),
  gender: Yup.string().required('Required'),
  weight: Yup.number().required('Required').positive('Must be positive'),
  height: Yup.number().required('Required').positive('Must be positive'),
  dateOfBirth: Yup.date()
    .required('Required')
    .test(
      'DOB',
      'You must be at least 15 years old',
      (value) => {
        return value ? addYears(new Date(), -15) >= new Date(value) : false;
      }
    ),
});

function Register() {
  const navigate = useNavigate();
  const [error, setError] = useState('');

  const handleSubmit = async (values) => {
    try {
      const response = await axios.post('http://localhost:5007/api/User/AddNewUser', null, {
        params: {
          name: values.name,
          email: values.email,
          pass: values.password,
          gender: values.gender,
          weight: values.weight,
          height: values.height,
          date: values.dateOfBirth
        }
      });
      console.log(response);
      navigate('/');
      alert("User Registered Successfully!");
    } catch (err) {
      setError('Registration failed. Email already exists!');
    }
  };

  return (
    <div className="min-h-screen flex">
      <motion.div 
        initial={{ x: -100, opacity: 0 }}
        animate={{ x: 0, opacity: 1 }}
        transition={{ duration: 0.5 }}
        className="w-1/2 flex items-center justify-center bg-white"
      >
        <div className="max-w-md w-full px-6">
          <div className="text-center mb-8">
            <Dumbbell className="w-12 h-12 mx-auto text-blue-600" />
            <h2 className="mt-6 text-3xl font-bold text-gray-900">Join FitTrack Today</h2>
            {/* <p className="mt-2 text-sm text-gray-600">Start your fitness journey today</p> */}
          </div>

          <Formik
            initialValues={{
              name: '',
              email: '',
              password: '',
              gender: '',
              weight: '',
              height: '',
              dateOfBirth: '',
            }}
            validationSchema={registerSchema}
            onSubmit={handleSubmit}
          >
            {({ errors, touched }) => (
              <Form className="space-y-4">
                <div>
                  <Field
                    name="name"
                    placeholder="Full Name"
                    className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"
                  />
                  {errors.name && touched.name && (
                    <div className="text-red-500 text-sm">{errors.name}</div>
                  )}
                </div>

                <div>
                  <Field
                    name="email"
                    type="email"
                    placeholder="Email"
                    className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"
                  />
                  {errors.email && touched.email && (
                    <div className="text-red-500 text-sm">{errors.email}</div>
                  )}
                </div>

                <div>
                  <Field
                    name="password"
                    type="password"
                    placeholder="Password"
                    className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"
                  />
                  {errors.password && touched.password && (
                    <div className="text-red-500 text-sm">{errors.password}</div>
                  )}
                </div>

                <div>
                  <Field
                    as="select"
                    name="gender"
                    className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"
                  >
                    <option value="">Select Gender</option>
                    <option value="male">Male</option>
                    <option value="female">Female</option>
                    <option value="other">Other</option>
                  </Field>
                  {errors.gender && touched.gender && (
                    <div className="text-red-500 text-sm">{errors.gender}</div>
                  )}
                </div>

                <div className="flex gap-4">
                  <div className="flex-1">
                    <Field
                      name="weight"
                      type="number"
                      placeholder="Weight (kg)"
                      className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"
                    />
                    {errors.weight && touched.weight && (
                      <div className="text-red-500 text-sm">{errors.weight}</div>
                    )}
                  </div>

                  <div className="flex-1">
                    <Field
                      name="height"
                      type="number"
                      placeholder="Height (cm)"
                      className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"
                    />
                    {errors.height && touched.height && (
                      <div className="text-red-500 text-sm">{errors.height}</div>
                    )}
                  </div>
                </div>

                <div>
                  <Field
                    name="dateOfBirth"
                    type="date"
                    className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"
                  />
                  {errors.dateOfBirth && touched.dateOfBirth && (
                    <div className="text-red-500 text-sm">{errors.dateOfBirth}</div>
                  )}
                </div>

                {error && (
                  <div className="text-red-500 text-sm text-center">{error}</div>
                )}

                <button
                  type="submit"
                  className="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700"
                >
                  Create Account
                </button>
                <p className="text-center text-sm text-gray-600">
                  Already have an account?{' '}
                  <Link to="/" className="font-medium text-blue-600 hover:text-blue-500">
                    Sign In
                  </Link>
                </p>
              </Form>
            )}
            
          </Formik>
        </div>
      </motion.div>

      <motion.div 
        initial={{ x: 100, opacity: 0 }}
        animate={{ x: 0, opacity: 1 }}
        transition={{ duration: 0.5 }}
        className="hidden lg:flex lg:w-1/2 bg-cover bg-center items-center justify-center p-12"
        style={{
          backgroundImage: 'linear-gradient(rgba(0, 0, 0, 0.5), rgba(0, 0, 0, 0.5)), url("https://images.unsplash.com/photo-1517836357463-d25dfeac3438?ixlib=rb-1.2.1&auto=format&fit=crop&w=1950&q=80")',
          backgroundBlendMode: 'overlay'
        }}
      />
    </div>
  );
}

export default Register;





