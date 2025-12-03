import { create } from 'zustand';
import type { User } from '../types/auth.types';
import authService from '../services/AuthService';

interface AuthState {
  user: User | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  
  // Actions
  setUser: (user: User | null) => void;
  setLoading: (loading: boolean) => void;
  initializeAuth: () => void;
  clearAuth: () => void;
}

export const useAuthStore = create<AuthState>((set) => ({
  user: null,
  isAuthenticated: false,
  isLoading: true,

  setUser: (user) => set({ 
    user, 
    isAuthenticated: !!user 
  }),

  setLoading: (isLoading) => set({ isLoading }),

  initializeAuth: () => {
    const user = authService.getUser();
    const isAuthenticated = authService.isAuthenticated();
    
    set({ 
      user, 
      isAuthenticated,
      isLoading: false 
    });
  },

  clearAuth: () => set({ 
    user: null, 
    isAuthenticated: false 
  }),
}));
