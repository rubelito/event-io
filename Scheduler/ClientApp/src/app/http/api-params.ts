import { HttpParams, HttpUrlEncodingCodec } from '@angular/common/http';

// Angular 13's default codec leaves '+' unescaped; form-style server parsers
// interpret it as a space. Encode it along with other query delimiters.
class ApiParameterCodec extends HttpUrlEncodingCodec {
  override encodeKey(key: string): string {
    return encodeURIComponent(key);
  }

  override encodeValue(value: string): string {
    return encodeURIComponent(value);
  }
}

export function createApiParams(): HttpParams {
  return new HttpParams({ encoder: new ApiParameterCodec() });
}
