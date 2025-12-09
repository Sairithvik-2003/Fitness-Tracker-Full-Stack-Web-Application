import Sidebar from './Sidebar';

function Layout({ children }) {
  return (
    <div className="flex min-h-screen bg-gray-100">
      <Sidebar />
      <main className="flex-1 ml-[70px] p-8">
        {children}
      </main>
    </div>
  );
}

export default Layout;
