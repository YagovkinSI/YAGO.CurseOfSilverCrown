import React from "react";
import { useNavigate } from 'react-router-dom';
import { AlertCircle } from 'lucide-react';
import YagoCard from './YagoCard';
import type { FetchBaseQueryError } from '@reduxjs/toolkit/query';
import type { SerializedError } from '@reduxjs/toolkit';
import IconAnimated from './IconAnimated';
import YagoTitle from './YagoTitle';
import YagoButton from './YagoButton';

interface ErrorCardProps {
  error: FetchBaseQueryError | SerializedError | string
}

const ErrorCard: React.FC<ErrorCardProps> = ({ error }) => {
  const navigate = useNavigate();

  function isFetchBaseQueryError(error: unknown): error is FetchBaseQueryError {
    return typeof error == 'object' && error != null && 'status' in error;
  }

  function isErrorWithStatus(error: unknown, status: number): error is FetchBaseQueryError {
    return isFetchBaseQueryError(error) && error.status == status;
  }

  const getErrorText = (error: FetchBaseQueryError | SerializedError | string): string => {
    if (typeof error === 'string')
      return error
    if (typeof error === 'object' && 'error' in error && typeof error.error === 'string' &&
      error.error == "TypeError: Failed to fetch")
      return 'Ошибка получения данных с сервера'
    if (typeof error === 'object' && 'data' in error && typeof error.data === 'string')
      return error.data;
    if (typeof error === 'object' && 'data' in error && typeof error.data === 'object' &&
      error.data && 'title' in error.data && typeof error.data.title === 'string') {
      return error.data.title;
    }
    if (isErrorWithStatus(error, 401))
      return 'Необходима авторизация.';
    if (isErrorWithStatus(error, 403))
      return 'Недостаточно прав.';
    return 'Неизвестная ошибка'
  }

  return (
    <YagoCard variant="error" className="flex flex-col items-center gap-4">
      <IconAnimated icon={AlertCircle} color="danger" size="lg" pingOpacity={0.2} />
      <YagoTitle>Ошибка</YagoTitle>
      <p className="text-muted text-center text-sm">
        {getErrorText(error)}
      </p>
      <div className="flex gap-3 w-full max-w-xs">
        <YagoButton size="sm" onClick={() => window.location.reload()} >
          Обновить
        </YagoButton>
        <YagoButton variant="secondary" size="sm" onClick={() => navigate('/')} >
          Выйти
        </YagoButton>
      </div>
    </YagoCard>)
}

export default ErrorCard;