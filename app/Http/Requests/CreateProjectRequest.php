<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;

class CreateProjectRequest extends FormRequest
{
    /**
     * Get the validation rules that apply to the request.
     *
     * @return array<string, mixed>
     */
    public function rules()
    {
        return [
            'name' => ['required', 'string'],
            'description' => ['required'],
            'logo' => ['required', 'string'],
            'url' => ['required', 'url'],
            'url_placeholder' => ['required', 'string']
        ];
    }
}
