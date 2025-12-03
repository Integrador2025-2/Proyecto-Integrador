import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuthStore } from '../store/authStore';
import authService from '../services/AuthService';
import type { LoginRequest, RegisterRequest, ChangePasswordRequest } from '../types/auth.types';

export const useAuth = () => {
  const navigate = useNavigate();
  const { user, isAuthenticated, isLoading, setUser, clearAuth, setLoading } = useAuthStore();

  const login = useCallback(async (credentials: LoginRequest) => {
    try {
      setLoading(true);
      const response = await authService.login(credentials);
      setUser(response.user);
      return response;
    } catch (error) {
      clearAuth();
      throw error;
    } finally {
      setLoading(false);
    }
  }, [setUser, clearAuth, setLoading]);

  const register = useCallback(async (userData: RegisterRequest) => {
    try {
      setLoading(true);
      const response = await authService.register(userData);
      setUser(response.user);
      return response;
    } catch (error) {
      clearAuth();
      throw error;
    } finally {
      setLoading(false);
    }
  }, [setUser, clearAuth, setLoading]);

  const loginWithGoogle = useCallback(async (googleToken: string) => {
    try {
      setLoading(true);
      const response = await authService.loginWithGoogle(googleToken);
      setUser(response.user);
      return response;
    } catch (error) {
      clearAuth();
      throw error;
    } finally {
      setLoading(false);
    }
  }, [setUser, clearAuth, setLoading]);

  const logout = useCallback(async () => {
    try {
      setLoading(true);
      await authService.logout();
      clearAuth();
      navigate('/login');
    } catch (error) {
      console.error('Logout error:', error);
      clearAuth();
      navigate('/login');
    } finally {
      setLoading(false);
    }
  }, [clearAuth, navigate, setLoading]);

  const changePassword = useCallback(async (passwords: ChangePasswordRequest) => {
    try {
      setLoading(true);
      await authService.changePassword(passwords);
    } finally {
      setLoading(false);
    }
  }, [setLoading]);

  const refreshUser = useCallback(async () => {
    try {
      setLoading(true);
      const updatedUser = await authService.getCurrentUser();
      setUser(updatedUser);
      return updatedUser;
    } catch (error) {
      console.error('Error refreshing user:', error);
      throw error;
    } finally {
      setLoading(false);
    }
  }, [setUser, setLoading]);

  const hasRole = useCallback((roleNames: string | string[]) => {
    return authService.hasRole(roleNames);
  }, []);

  return {
    user,
    isAuthenticated,
    isLoading,
    login,
    register,
    loginWithGoogle,
    logout,
    changePassword,
    refreshUser,
    hasRole,
  };
};
